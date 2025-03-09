using E_Learning.Common;
using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;

namespace E_Learning.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PayOSController : ControllerBase
    {

        private readonly PayOS _payOS;
        private readonly PaymentRepository paymentRepository;
        private readonly EnrollmentRepository enrollmentRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        public PayOSController(PayOS payOS, PaymentRepository paymentRepository, 
                EnrollmentRepository enrollmentRepository, IHttpContextAccessor httpContextAccessor)
        {
            _payOS = payOS;
            this.paymentRepository = paymentRepository;
            this.enrollmentRepository = enrollmentRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentLink(CreatePaymentLinkRequest body)
        {
            try
            {
                int orderCode = body.orderCode;
                ItemData item = new ItemData(body.productName, 1, body.price);
                List<ItemData> items = new List<ItemData>();
                items.Add(item);
                PaymentData paymentData = new PaymentData(orderCode, body.price, body.description, items, body.cancelUrl, body.returnUrl);

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userId == null)
                {
                    throw new AppException(ErrorCode.USER_NOT_EXISTED);
                }

                Payment payment = new Payment
                {
                    OrderCode = orderCode,
                    CourseId = body.courseId,
                    UserId = int.Parse(userId.Value),
                    PaymentMethod = PaymentMethod.ONLINE_BANKING,
                    PaymentGateWay = PaymentGateWay.PAYOS,
                    Currency = Currency.USD,
                    Status = PaymentStatus.PENDING,
                    Price = body.price,
                    CreatedAt = DateTime.Now,
                };

                await paymentRepository.CreatePayment(payment);

                return Ok(new PaymentResponse(0, "success", createPayment));
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new PaymentResponse(-1, "fail", null));
            }

        }

        [HttpPost("payos_transfer_handler")]
        [AllowAnonymous]
        public async Task<IActionResult> payOSTransferHandler(WebhookType body)
        {
            try
            {
                WebhookData data = _payOS.verifyPaymentWebhookData(body);
                string code = data.code;
                long orderCode = data.orderCode;
                if (code == "00")
                {
                    Payment payment = await paymentRepository.FindByOrderCode(orderCode);
                    if (payment == null)
                    {
                        return Ok(new PaymentResponse(-1, "fail", null));
                    }
                    payment.Status = PaymentStatus.SUCCESS;
                    Course course = payment.Course;
                    User user = payment.User;
                    Enrollment enrollment = new Enrollment
                    {
                        Course = course,
                        User = user,
                        Purchased = true,
                        IsComplete = false,
                        CreatedAt = DateTime.Now,
                    };

                    enrollmentRepository.Add(enrollment);

                    return Ok(new PaymentResponse(0, "Ok", null));
                }
                return Ok(new PaymentResponse(0, "Ok", null));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Ok(new PaymentResponse(-1, "fail", null));
            }
        }

    }
}
