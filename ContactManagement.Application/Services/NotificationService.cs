using ContactManagement.Application.Interfaces;
using ContactManagement.Application.Models;

namespace ContactManagement.Application.Services;

public class NotificationService : INotificationService
{
     private readonly List<Notification> _notifications;

     public NotificationService()
     {
         _notifications = new List<Notification>();
     }

     public void Handle(Notification notification)
     {
         _notifications.Add(notification);
     }

     public List<Notification> GetNotifications()
     {
         return _notifications;
     }

     public bool HasNotification()
     {
         return _notifications.Any();
     }
 }

