using Abstraction.Contract.Repository.Fund;
using Domain.Entities.FundManagement;

namespace Infrastructure.Data.Config
{
    public static class StatusHistoryConfig
    {

        public static async Task SeedStatusHistoryAsync(IStatusHistoryRepository statusHistoryRepository)
        {
            var statusHistories = new List<StatusHistory>
            {
                new StatusHistory
                {
                    NameAr = "تحت الإنشاء",
                    NameEn = "Under Construction"
                },
                new StatusHistory
                {
                    NameAr = "لا يوجد أعضاء",
                    NameEn = "Waiting for Adding Members"
                },
                new StatusHistory
                {
                    NameAr = "نشط",
                    NameEn = "Active"
                },
                new StatusHistory
                {
                    NameAr = "تم التخارج",
                    NameEn = "Exited"
                }
            };

            foreach (var status in statusHistories)
            {
                if (!await statusHistoryRepository.AnyAsync<StatusHistory>(x => x.Id == status.Id))
                {
                    await statusHistoryRepository.AddAsync(status);
                }
            }
        }
    }
}
