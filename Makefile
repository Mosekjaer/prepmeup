SHELL := /bin/bash
.DEFAULT_GOAL := help

CLIENTS := clients
AVD ?= $(shell emulator -list-avds 2>/dev/null | head -n 1)
EMULATOR_LOG ?= $(or $(TMPDIR),/tmp)/prepmeup-emulator.log
REVERSE_PORTS := 5001 8082 8081

.PHONY: help install up down logs admin mobile android reverse android-stop \
        build test typecheck client-test check api-client report clean db-reset

##@ General

help: ## Show this help
	@awk 'BEGIN {FS = ":.*## "} \
	  /^##@/ {printf "\n\033[1m%s\033[0m\n", substr($$0, 5)} \
	  /^[a-zA-Z_-]+:.*## / {printf "  \033[36m%-14s\033[0m %s\n", $$1, $$2}' $(MAKEFILE_LIST)
	@echo

install: $(CLIENTS)/node_modules ## Install client packages (also activates git hooks)

$(CLIENTS)/node_modules: $(CLIENTS)/package-lock.json
	cd $(CLIENTS) && npm install
	@touch $@

##@ Run

up: ## Start api, db and keycloak in the background (needs .env)
	docker compose up -d --build
	@echo "API: http://localhost:5001  Keycloak: http://localhost:8082"

down: ## Stop the compose stack (keeps the data)
	docker compose down

logs: ## Follow the compose logs
	docker compose logs -f

admin: install ## Start the admin panel with Vite on port 5173
	cd $(CLIENTS) && npm run dev -w @prepmeup/admin

mobile: install ## Start Metro and open the customer app on the Android device
	cd $(CLIENTS)/apps/mobile && npx expo start --android

##@ Android

android: ## Start the emulator headless and forward the ports (AVD=<name>)
	@if [ -n "$$(adb devices | awk 'NR>1 && $$2=="device"')" ]; then \
	  echo "A device is already connected."; \
	else \
	  [ -n "$(AVD)" ] || { echo "No AVD found. Create one in Android Studio."; exit 1; }; \
	  echo "Starting $(AVD) headless (log: $(EMULATOR_LOG))"; \
	  nohup emulator -avd "$(AVD)" -no-window -gpu host -no-boot-anim >"$(EMULATOR_LOG)" 2>&1 & \
	  adb wait-for-device; \
	  until [ "$$(adb shell getprop sys.boot_completed 2>/dev/null | tr -d '\r')" = 1 ]; do sleep 1; done; \
	  echo "Emulator booted."; \
	fi
	@$(MAKE) --no-print-directory reverse

reverse: ## Forward API (5001), Keycloak (8082) and Metro (8081) to the device
	@for port in $(REVERSE_PORTS); do adb reverse tcp:$$port tcp:$$port >/dev/null && echo "localhost:$$port -> device"; done

android-stop: ## Stop the emulator
	adb emu kill

##@ Quality (same steps as CI)

build: ## Build the backend in Release
	dotnet build --configuration Release

test: ## Run unit and integration tests
	dotnet test --configuration Release

typecheck: install ## Typecheck all client packages
	cd $(CLIENTS) && npm run typecheck

client-test: install ## Run Vitest in all client packages
	cd $(CLIENTS) && npm test

check: build test typecheck client-test ## Run everything CI runs

api-client: install ## Regenerate api-client from the OpenAPI document
	cd $(CLIENTS) && npm run generate:api-client

report: ## Build the report with latexmk (needs docs/latexmkrc)
	cd docs && latexmk

##@ Clean up

clean: ## Delete build output (bin, obj, build, dist, .expo), never .env
	@find . \( -path ./.git -o -name node_modules -o -path ./context -o -path ./video \) -prune -o \
	  -type d \( -name bin -o -name obj -o -name build -o -name dist -o -name .expo \) -prune -print \
	  | git check-ignore --stdin | tee /dev/stderr | xargs -r rm -rf

db-reset: ## Delete the local database volume (asks first)
	@read -p "Delete all data in the local database? [y/N] " answer && [ "$$answer" = y ]
	docker compose down
	docker volume rm prepmeup-data
