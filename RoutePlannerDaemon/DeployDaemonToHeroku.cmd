call heroku login
call heroku container:login
call heroku container:push worker --app route-updater --context-path ..
call heroku container:release worker --app route-updater
call heroku open -a route-updater
call heroku logs --tail -a route-updater