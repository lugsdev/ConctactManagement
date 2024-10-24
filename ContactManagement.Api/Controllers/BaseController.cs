using ContactManagement.Api.Extensions.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ContactManagement.Application.Models;
using ContactManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace ContactManagement.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        protected BaseController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        protected bool OperacaoValida()
        {
            return !_notificationService.HasNotification();
        }

        protected ActionResult CustomResponse(HttpStatusCode httpStatusCode = HttpStatusCode.OK, object? result = null)
        {
            if (OperacaoValida())
            {
                // Se a operação é válida, retorna o resultado com o status apropriado.
                if (httpStatusCode == HttpStatusCode.NoContent)
                    return new NoContentResult();

                return new ObjectResult(result)
                {
                    StatusCode = (int)httpStatusCode,
                };
            }

            // Se houver notificações de erro, capturá-las.
            var mensagens = _notificationService.GetNotifications();
            var mensagensNotification = mensagens?.Select(n => n.Message).ToList() ?? new List<string>();

            // Pega o status code definido na notificação ou assume 400 por padrão.
            var statusCodeNotification = mensagens?.FirstOrDefault()?.HttpStatusCode ?? HttpStatusCode.BadRequest;

            // Retorna uma resposta de erro personalizada.
            return StatusCode((int)statusCodeNotification, new ErrorResponse
            {
                StatusCode = (int)statusCodeNotification,
                Errors = mensagensNotification
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotifyInvalidModelState(modelState);
            return CustomResponse();
        }

        protected void NotifyInvalidModelState(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(errorMsg);
            }
        }
        protected void NotifyError(string mensagem)
        {
            _notificationService.Handle(new Notification(mensagem));
        }

        protected ActionResult NotFound(string error)
        {
            var code = (int)HttpStatusCode.NotFound;

            return StatusCode(code, new ErrorResponse()
            {
                StatusCode = code,
                Errors = new List<string> { error }
            });
        }

    }
}
