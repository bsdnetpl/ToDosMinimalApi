namespace ToDosMinimalApi.ToDo
{
    public static class TodoReqests
    {
        public static WebApplication RegisterEndPoint(this WebApplication app)
        {
            //app.MapGet("/todos", (IToDoService service) => service.GetAll()); // stare wywolanie endpoint
            app.MapGet("/todos", TodoReqests.GetAll)
                .Produces<List<ToDo>>()
                .WithTags("To Doe");

            //app.MapGet("/todos/{id}", ([FromServices]IToDoService service, [FromRoute] Guid id) => service.GetById(id));
            app.MapGet("/todos/{id}", TodoReqests.GetById)
                .Produces<ToDo>()
                .Produces(StatusCodes.Status404NotFound)
                .WithTags("To Doe");

            //app.MapPost("/todos", (IToDoService service,ToDo toDo) => service.Create(toDo));
            app.MapPost("/todos", TodoReqests.Create)
                .Produces<ToDo>(StatusCodes.Status201Created)
                .Accepts<ToDo>("application/json")
                .WithTags("To Doe");

            //app.MapPut("/todos/{id}",(IToDoService service,Guid id,ToDo toDo ) =>service.Update(toDo));
            app.MapPut("/todos/{id}", TodoReqests.Update)
                .Produces<ToDo>(StatusCodes.Status204NoContent)
                .Produces<ToDo>(StatusCodes.Status404NotFound)
                .Accepts<ToDo>("application/json")
                .WithTags("To Doe");

            //app.MapDelete("/todos/{id}", (IToDoService service, Guid id) => service.Delete(id));
            app.MapDelete("/todos/{id}", TodoReqests.Delete)
                .Produces<ToDo>(StatusCodes.Status204NoContent)
                .Produces<ToDo>(StatusCodes.Status404NotFound)
                .WithTags("To Doe")
                .ExcludeFromDescription();// ukrywanie metody w swagger
            
            return app;
        }
        public static IResult GetAll(IToDoService service)
        {
            var Todos = service.GetAll();
            return Results.Ok(Todos);
        }

        public static IResult GetById(IToDoService service, Guid id)
        {
            var todo =service.GetById(id);
            if(todo == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(todo);
        }

        public static IResult Create(IToDoService service, ToDo toDo)
        {
            service.Create(toDo);
            return Results.Created($"/todos{toDo.Id}",toDo);

        }

        public static IResult Update(IToDoService service, Guid id, ToDo toDo)
        {
            var todo  = service.GetById(id);
            if (todo == null)
            {
                return Results.NotFound();
            }
            service.Update(todo);
            return Results.NoContent();
        }
        public static IResult Delete(IToDoService service, Guid id)
        {
            var todo = service.GetById(id);
            if (todo == null)
            {
                return Results.NotFound();
            }
            service.Delete(id);
            return Results.NoContent();
        }

    }
 }

