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




Exemplo de console app par usar o servo em C#

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Device.Pwm;
using System.Device.Pwm.Drivers;
using Iot.Device.ServoMotor;

Console.WriteLine("Hello Servo Motor!");

using PwmChannel pwmChannel = PwmChannel.Create(0, 0, 50);
var spwmChannel = new SoftwarePwmChannel(18, frequency: 50, 0.5,true);
using ServoMotor servoMotor = new ServoMotor(pwmChannel, 180);
pwmChannel.Start();
servoMotor.Calibrate(180, 0.5, 2.5);

// Samples.
WriteAngle(pwmChannel, servoMotor);
// WriteAngle(pwmChannel, servoMotor);
// Methods
void WritePulseWidth(PwmChannel pwmChannel, ServoMotor servoMotor)
{
    //servoMotor.Calibrate(180, 0.5, 2.5);
    servoMotor.Start();
    
    while (true)
    {
        Console.WriteLine("Enter a pulse width in microseconds ('Q' to quit). ");
        string? pulseWidth = Console.ReadLine();

        if (pulseWidth?.ToUpper() is "Q" or null)
        {
            break;
        }

        if (!int.TryParse(pulseWidth, out int pulseWidthValue))
        {
            Console.WriteLine($"Can not parse {pulseWidth}.  Try again.");
        }

        servoMotor.WritePulseWidth(pulseWidthValue);
        //Console.WriteLine($"Duty Cycle: {pwmChannel.DutyCycle * 100.0}%");
    }

    servoMotor.Stop();
}

void WriteAngle(PwmChannel pwmChannel, ServoMotor servoMotor)
{
    servoMotor.Start();

    while (true)
    {
        Console.WriteLine("Enter an angle ('Q' to quit). ");
        string? angle = Console.ReadLine();

        if (angle?.ToUpper() is "Q" or null)
        {
            break;
        }

        if (!int.TryParse(angle, out int angleValue))
        {
            Console.WriteLine($"Can not parse {angle}.  Try again.");
        }

        servoMotor.WriteAngle(angleValue);
        //Console.WriteLine($"Duty Cycle: {pwmChannel.DutyCycle * 100.0}%");
    }

    servoMotor.Stop();
}
