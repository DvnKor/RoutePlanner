call heroku login
call heroku container:login
call docker build -t route-planner-api .
call heroku container:push -a route-planner-api web
call heroku container:release -a route-planner-api web
call heroku open -a route-planner-api
call heroku logs --tail -a route-planner-api