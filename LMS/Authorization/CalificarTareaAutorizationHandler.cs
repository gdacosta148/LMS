using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace LMS.Authorization
{
    public class CalificarTareaAutorizationHandler : AuthorizationHandler<CalificarTareaAutorization, Calificaciones>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                    CalificarTareaAutorization requirement,
                                                    Calificaciones resource)
        {
            var Iduser = resource.Tareas.Prof.UserId;

            if (context.User.HasClaim(ClaimTypes.NameIdentifier, Iduser))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

    }
}
