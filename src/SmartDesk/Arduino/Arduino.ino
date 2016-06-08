const int triggerPin = 7;
const int echoPin = 6;

void setup() {
  // Init serial
  Serial.begin(9600);
  // Init pins
  pinMode(triggerPin, OUTPUT);
  pinMode(echoPin, INPUT);
}

void loop() {
  byte inputCmd = 0;
  long distance;
  //Check if there is something to read
  if (Serial.available() == 1) {
    //Read buffer
    inputCmd = Serial.read();

    //Check for Cmd
    if (inputCmd == 'R') {
      distance = getDistance();
      if (distance >= 500 || distance <= 0)
        Serial.println("-1");
      else
        Serial.println(distance);
    }
    else {
      Serial.print("Unkown commamd: ");
      Serial.println((char)inputCmd);
    }
    //Clear Cmd bytes
    inputCmd = 0;
  }
}

// Evaluate distance
int getDistance() {
  long duration = 0;
  digitalWrite(triggerPin, LOW);
  delay(5);
  digitalWrite(triggerPin, HIGH);
  delay(10);
  digitalWrite(triggerPin, LOW);
  duration = pulseIn(echoPin, HIGH);

  return (duration / 2) / 29.1;
}

