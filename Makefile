PROJECT_NAME= weather-app
HASH=$(shell git rev-parse HEAD)

FRONTEND_NAME= $(PROJECT_NAME)-frontend
BACKEND_NAME= $(PROJECT_NAME)-backend

client-dev:
	@npm run --prefix Client start

server-dev:
	@dotnet watch --project Server run

deploy:
	# deploy client
	@docker build --rm \
		-t ${DOCKER_USERNAME}/$(FRONTEND_NAME):latest \
		-t ${DOCKER_USERNAME}/$(FRONTEND_NAME):$(HASH) \
		-f Client/build/prod/Dockerfile Client

	@docker push ${DOCKER_USERNAME}/$(FRONTEND_NAME):latest
	@docker push ${DOCKER_USERNAME}/$(FRONTEND_NAME):$(HASH)

	@kubectl set image \
		deployments/$(FRONTEND_NAME) \
		$(FRONTEND_NAME)=${DOCKER_USERNAME}/$(FRONTEND_NAME):$(HASH)

	# deploy server
	@docker build --rm \
		-t ${DOCKER_USERNAME}/$(BACKEND_NAME):latest \
		-t ${DOCKER_USERNAME}/$(BACKEND_NAME):$(HASH) \
		-f Server/Dockerfile Server

	@docker push ${DOCKER_USERNAME}/$(BACKEND_NAME):latest
	@docker push ${DOCKER_USERNAME}/$(BACKEND_NAME):$(HASH)

	@kubectl set image \
		deployments/$(BACKEND_NAME) \
		$(BACKEND_NAME)=${DOCKER_USERNAME}/$(BACKEND_NAME):$(HASH)
