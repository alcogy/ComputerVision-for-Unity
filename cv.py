import cv2
import socket
import mediapipe as mp
import math

# UDP socket
udp = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
server = ("127.0.0.1", 9999)

# 3D point
class Vertex:
    x = None
    y = None
    z = None

cap = cv2.VideoCapture(0)
hands = mp.solutions.hands.Hands()
prev_point = Vertex()

while True:
    res, img = cap.read()
    rgb = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
    results = hands.process(rgb)
    if results.multi_hand_landmarks:
        for mhl in results.multi_hand_landmarks:
            for id, lm in enumerate(mhl.landmark):
                if id == 8:
                    if prev_point.x is not None:
                        dx = lm.x - prev_point.x
                        dy = lm.y - prev_point.y
                        dz = lm.z - prev_point.z

                        udp.sendto(str.encode(str(-dx) + "," + str(-dy) + "," + str(-dz)), server)
                        # absolute points
                        # udp.sendto(str.encode(str(lm.x) + "," + str(lm.y) + "," + str(lm.z)), server)

                    prev_point.x = lm.x
                    prev_point.y = lm.y
                    prev_point.z = lm.z
    
    # Preview
    # cv2.imshow("preview", img)
    cv2.waitKey(1)

