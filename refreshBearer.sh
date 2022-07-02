$yandexPassportOauthToken = $(dotnet user-secrets list | grep Yandex | awk '{print $3}')
$bearer = $(curl -d "{\"yandexPassportOauthToken\":\"$yandexPassportOauthToken"}" "https://iam.api.cloud.yandex.net/iam/v1/tokens")
dotnet user-secrets set Bearer $bearer