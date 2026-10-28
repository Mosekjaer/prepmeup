#!/bin/sh
set -eu

: "${PMU_API_PUBLIC_URL:?set PMU_API_PUBLIC_URL}"
: "${PMU_KEYCLOAK_PUBLIC_URL:?set PMU_KEYCLOAK_PUBLIC_URL}"

envsubst '${PMU_API_PUBLIC_URL} ${PMU_KEYCLOAK_PUBLIC_URL}' \
  < /opt/admin/config.js.template \
  > /tmp/config.js
