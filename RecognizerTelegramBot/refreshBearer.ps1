$yandexPassportOauthToken = @( dotnet user-secrets list | %{ $_.Split(' ')[2]; })[-2]
$Body = @{ yandexPassportOauthToken = "$yandexPassportOauthToken" } | ConvertTo-Json -Compress
$bearer = Invoke-RestMethod -Method 'POST' -Uri 'https://iam.api.cloud.yandex.net/iam/v1/tokens' -Body $Body -ContentType 'Application/json' | Select-Object -ExpandProperty iamToken
dotnet user-secrets set Bearer $bearer