# RecognizerTelegramBot

Бот для Telegram, распознающий голосовые сообщения длительностью до 30 секунд. Для распознавания использован Yandex SpeechKit. 

[\*жмак\*](https://t.me/notfoundexceptionbot)
<br>
<br>
Токены находятся в пользовательских секретах:
* YandexOauthToken - oatuh-токен, использующийся для генерации Bearer-токена.
* TelegramToken - токен бота, получаемый от [BotFather](https://t.me/botfather).
* Bearer - токен для авторизации в Yandex Cloud.

Bearer токен генерируется скриптами [refreshBearer.ps1](https://github.com/nsbrokeboy/RecognizerTelegramBot/blob/main/RecognizerTelegramBot/refreshBearer.ps1) (PowerShell), а также [refreshBearer.sh](https://github.com/nsbrokeboy/RecognizerTelegramBot/blob/main/RecognizerTelegramBot/refreshBearer.sh) (Bash). Время жизни - 12 часов, рекомендуется автоматизировать выполнение скрипта для непрерывной работы бота.
