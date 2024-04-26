using System.Device.Pwm;
using System.Device.Pwm.Drivers;
using Iot.Device.ServoMotor;
using TestProject.Infrastructure.Controllers;



//CONTROLLER
var inputMonitor = new XboxGamepad();
inputMonitor.testEvento += onTestAction;
inputMonitor.Start();

using PwmChannel pwmChannel = PwmChannel.Create(0, 0, 50);
var spwmChannel = new SoftwarePwmChannel(18, frequency: 50, 0.5,true);
using ServoMotor servoMotor = new ServoMotor(spwmChannel);

// Samples.
WritePulseWidth(pwmChannel, servoMotor);
// WriteAngle(pwmChannel, servoMotor);
// Methods

static void onTestAction(object source, string message)
{
    Console.WriteLine(message);
}

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
        Console.WriteLine($"Duty Cycle: {pwmChannel.DutyCycle * 100.0}%");
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