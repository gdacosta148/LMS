using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LMS.Authorization
{
    public class ProfesorCursoAuthorizationHandler: AuthorizationHandler<ProfesorCursoAuthorization, Curso>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                    ProfesorCursoAuthorization requirement,
                                                      Curso resource)
        {
            var Iduser = resource.Profesor.UserId;

            if (context.User.HasClaim(ClaimTypes.NameIdentifier, Iduser))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }



    }
}
