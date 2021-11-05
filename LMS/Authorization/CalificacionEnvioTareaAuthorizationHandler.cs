using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LMS.Authorization
{
    public class CalificacionEnvioTareaAuthorizationHandler: AuthorizationHandler<CalificacionEnvioTareaAutorization,Calificaciones>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       CalificacionEnvioTareaAutorization requirement,
                                                       Calificaciones resource)
        {
            var Iduser = resource.Estudiante.UserId;

            if (context.User.HasClaim(ClaimTypes.NameIdentifier, Iduser))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }



    }
}
