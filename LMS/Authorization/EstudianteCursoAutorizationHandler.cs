using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LMS.Authorization
{
    public class EstudianteCursoAutorizationHandler: AuthorizationHandler<EstudianteCursoAutorization, Estudiantes>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                    EstudianteCursoAutorization requirement,
                                                      Estudiantes resource)
        {
            var Iduser = resource.UserId;

            if (context.User.HasClaim(ClaimTypes.NameIdentifier, Iduser))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }

        }
}
