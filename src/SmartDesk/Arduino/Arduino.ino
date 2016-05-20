int trigger = 7;
int echo = 6;
long dauer = 0;

long entfernung = 0;
byte inputByte = 0;


void setup()
{
  Serial.begin(9600);

  pinMode(trigger, OUTPUT);
  pinMode(echo, INPUT);
}

void loop() {

  //Read Buffer
  if (Serial.available() == 5)
  {
    //Read buffer
    inputByte = Serial.read();

  }
  //Check for start of Message
  if (inputByte == 'R')
  {


    digitalWrite(trigger, LOW);

    delay(5);
    digitalWrite(trigger, HIGH);
    delay(10);
    digitalWrite(trigger, LOW);
    dauer = pulseIn(echo, HIGH);

    entfernung = (dauer / 2) / 29.1;

    if (entfernung >= 500 || entfernung <= 0)
    {
      Serial.println("-1\n");
    }
    else
    {
      Serial.print(entfernung);
      Serial.println("\n");
    }
    //delay(1000);
  }
  //Clear Message bytes
  inputByte = 0;

}

