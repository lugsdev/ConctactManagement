using ContactManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Interfaces;
public interface INotificationService
{
    bool HasNotification();

    List<Notification> GetNotifications();

    void Handle(Notification notification);
}

