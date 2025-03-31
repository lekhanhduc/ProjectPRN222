using E_Learning.Common;

namespace E_Learning.Dto.Response.Admin
{
    public class WithdrawalHistoryResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long Money { get; set; }

        public string Qr { get; set; }

        public TransactionStatus Status { get; set; }

        // Định dạng ngày tháng (yyyy-MM-dd HH:mm:ss)
        public DateTime CreatedAt { get; set; }
    }
}
