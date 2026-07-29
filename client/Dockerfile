# syntax=docker/dockerfile:1

FROM node:22-alpine AS build
WORKDIR /app
COPY package.json package-lock.json ./
RUN --mount=type=cache,id=npm,target=/root/.npm \
    npm ci
COPY . .
RUN npm test
RUN npm run build

FROM scratch AS dist
COPY --from=build /app/dist /dist
