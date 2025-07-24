using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.Contract.Repository.Notifications;
using Abstraction.Contracts.Repository;
using Domain.Entities.Notifications;

namespace Application.Features.Notification
{
    public class FundNotificationObserver : IFundNotificationObserver
    {
        private readonly IRepositoryManager _repository;
        public FundNotificationObserver(IRepositoryManager repository)
        {
            _repository = repository;
        }
        
        public async Task OnSendNotification(MessageRequest message)
        {
            await _repository.Notifications.SendNotification(message);
        }
    }
}
