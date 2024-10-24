using ContactManagement.Application.Interfaces;
using ContactManagement.Application.Models;
using ContactManagement.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using System.Net;

namespace ContactManagement.Application.Services;

public abstract class BaseService
{
    private readonly INotificationService _notificationService;

    protected BaseService(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    protected void Notify(ValidationResult validationResult)
    {
        foreach (var item in validationResult.Errors)
        {
            Notify(item.ErrorMessage);
        }
    }

    protected void Notify(string mensagem, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        _notificationService.Handle(new Notification(mensagem, statusCode));
    }

    protected bool ExecutarValidacao<TV, TE>(TV validation, TE entity)
        where TV : AbstractValidator<TE>
        where TE : Entity
    {
        var validator = validation.Validate(entity);

        if (validator.IsValid) return true;

        Notify(validator);

        return false;
    }
}