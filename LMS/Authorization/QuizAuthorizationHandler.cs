using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LMS.Authorization
{
    public class QuizAuthorizationHandler: AuthorizationHandler<QuizAuthorization, Quiz>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                              QuizAuthorization requirement,
                                                Quiz resource)
        {
            var Iduser = resource.Curso.Profesor.UserId;

            if (context.User.HasClaim(ClaimTypes.NameIdentifier, Iduser))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }


    }
}
