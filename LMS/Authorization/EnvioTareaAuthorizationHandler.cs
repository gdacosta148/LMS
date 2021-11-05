using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LMS.Authorization
{
    public class EnvioTareaAuthorizationHandler: AuthorizationHandler<EnvioTareaAuthorization,EnvioTarea>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                        EnvioTareaAuthorization requirement,
                                                        EnvioTarea resource)
        {
            
            var Iduser = resource.Calificacion.Estudiante.UserId;

            if (context.User.HasClaim(ClaimTypes.NameIdentifier, Iduser))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }


    }
}
