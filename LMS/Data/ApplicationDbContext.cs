using System;
using System.Collections.Generic;
using System.Text;
using LMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }




        public DbSet<Clase> Clase { get; set; }
        public DbSet<Materia> Materia { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<Profesores> Profesores { get; set; }

        public DbSet<Estudiantes> Estudiantes { get; set; }

        public DbSet<Tareas> Tareas { get; set; }

        public DbSet<Calificaciones> Calificaciones { get; set; }

        public DbSet<ClaseProfesor> ClaseProfesors { get; set; }

        public DbSet<MateriaProfesor> MateriaProfesors { get; set; }

        //public DbSet<TareaMateria> TareaMaterias { get; set; }

        //public DbSet<ClaseTarea> ClaseTareas { get; set; }

        //public DbSet<CalificaciónClase> CalificacionClase { get; set; }
        public DbSet<Curso> Curso { get; set; }

        public DbSet<CursoClase> CursoClases {get; set;}

        public DbSet <EnvioTarea> EnvioTarea { get; set; }

        public DbSet<Quiz> Quiz { get; set; }


        public DbSet<QuizQuestion> QuizQuestion { get; set; }



        public DbSet<QuizAnswers> QuizAnswers { get; set; }

        public DbSet<QuizOptions> QuizOptions { get; set; }

        public DbSet<Temp> Temp { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ClaseProfesor>()
                .HasOne(c => c.Clases)
                .WithMany(p => p.ClaseProfesor)
                .HasForeignKey(f => f.ClaseId);

            builder.Entity<ClaseProfesor>()
              .HasOne(c => c.Profesores)
              .WithMany(p => p.ClaseProfesor)
              .HasForeignKey(f => f.ProfesorId);

     

            //builder.Entity<EnvioTarea>().
            //    HasOne(t => t.Tarea).
            //    WithMany(e => e.EnvioTareas).
            //    HasForeignKey(t => t.TareaId);


            //builder.Entity<MateriaProfesor>()
            //    .HasKey(c => new { c.ProfesorId, c.MateriaId });

            builder.Entity<MateriaProfesor>()
                .HasOne(c => c.Materias)
                .WithMany(p => p.MateriaProfesor)
                .HasForeignKey(f => f.MateriaId);

            builder.Entity<MateriaProfesor>()
                .HasOne(c => c.Profesores)
                .WithMany(m => m.MateriaProfesor)
                .HasForeignKey(f => f.ProfesorId);



            builder.Entity<CursoClase>()
            .HasOne(c => c.Clase)
            .WithMany(m => m.CursoClase)
            .HasForeignKey(f => f.ClaseId);

            builder.Entity<CursoClase>()
                .HasOne(c => c.Curso)
                .WithMany(m => m.CursoClases)
                .HasForeignKey(f => f.CursoId);
       






        }


    }
}
