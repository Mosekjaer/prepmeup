FROM node:24-alpine AS build
WORKDIR /clients

COPY package.json package-lock.json tsconfig.base.json ./
COPY apps/admin/package.json apps/admin/
COPY apps/mobile/package.json apps/mobile/
COPY packages/api-client/package.json packages/api-client/
RUN npm ci --workspace @prepmeup/admin

COPY apps/admin apps/admin
COPY packages/api-client packages/api-client
RUN npm run build --workspace @prepmeup/admin

FROM nginxinc/nginx-unprivileged:1.29-alpine AS runtime

COPY --from=build /clients/apps/admin/dist /usr/share/nginx/html
COPY apps/admin/deploy/nginx.conf /etc/nginx/conf.d/default.conf
COPY apps/admin/deploy/config.js.template /opt/admin/config.js.template
COPY --chmod=755 apps/admin/deploy/40-write-config.sh /docker-entrypoint.d/40-write-config.sh

EXPOSE 8080
