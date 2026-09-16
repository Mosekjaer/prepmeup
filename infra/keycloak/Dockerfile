# Bakes the realm into the image so the import does not depend on a bind mount.
# Coolify resolves relative mount paths against its own working directory rather
# than the cloned repo, which silently turned the mount target into an empty
# directory and left the realm unimported.
FROM quay.io/keycloak/keycloak:26.3

COPY realm-prepmeup.json /opt/keycloak/data/import/realm-prepmeup.json
