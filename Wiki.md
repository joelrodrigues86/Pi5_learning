Pi5 Pinout image

![image](https://github.com/joelrodrigues86/Pi5_learning/assets/140548818/226dacd1-851a-4a93-8f2b-f796e2614d5b)


Ir para a configuração do Raspberry Pi para mudar keyboar layout, etc
raspi-config


RESTART
sudo reboot

SSH no PC
ls ~/.ssh -> Para verificar se já foi cridada uma chave ssh (se existe um ficheiro id_rsa.pub)

Gerar chave ssh
ssh-keygen

copiar chave ssh do pc para o rpi
cat ~/.ssh/id_rsa.pub | ssh pi@raspberrypi 'mkdir -p ~/.ssh && cat >> ~/.ssh/authorized_keys'

Deploy
dotnet publish --runtime linux-arm64 --self-contained

Copy deploy to Pi
scp -r D:\dev\Pi\TestProject\TestProject\TestProject\bin\Debug\net6.0-windows10.0.17763.0\linux-arm64\publish\* pi@raspberrypi:~/TestApp

Make dll executable
chmod +x ./TestProject

Enabling Hardware PWM on Raspberry Pi
https://github.com/dotnet/iot/blob/main/Documentation/raspi-pwm.md

Para dar enable ao PWM no PI5

1) Instalar todos os updates do OS do RPi5
2) Alterar o ficheiro config.txt e adicionar a seguinte linha no final
dtoverlay=pwm
3) ctrl+s para gravar
4) ctrl+x para sair
5) sudo reboot

LIGAR SERVO
Ligar ao pwm default (channel 0):
Vermelho (positivo) -> Pin 2 (5v)
Castanho (negativo) -> Pin 6 (negativo, ground)
Laranja (comunicação) -> Pin 12 / GPIO 18 (PWM)
