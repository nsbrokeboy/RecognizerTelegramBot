#!/bin/bash
yandexPassportOauthToken=$(dotnet user-secrets list | grep Yandex | awk '{print $3}')
bearer=$(curl -d "{\"yandexPassportOauthToken\":\"${yandexPassportOauthToken}\"}" "https://iam.api.cloud.yandex.net/iam/v1/tokens" | jq --raw-output '.iamToken')
dotnet user-secrets set Bearer $bearer
