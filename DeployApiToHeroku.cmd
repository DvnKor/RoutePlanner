call heroku login
call heroku container:login
call heroku container:push web -a route-planner-api
call heroku container:release web -a route-planner-api
call heroku open -a route-planner-api
call heroku logs --tail -a route-planner-api