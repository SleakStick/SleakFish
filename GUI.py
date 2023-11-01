import pygame
import socket
import threading

running = True
responseBool = False


#-------------------Request handler for communication with c# main.cs script------------------------
def requestHandler():
    global running
    global responseBool
    host, port = "127.0.0.1", 25001
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    try:
        sock.connect((host, port))
        print("successfully hosted socket on IP; "+host+" and port; ",port)
    except ConnectionRefusedError:
        print("Connection to client failed, main.cs script isn't running or has failed to run request handler")
        running = False

    while running:
        if not responseBool:        #Send request if we haven't yet received a response
            request = "pieces_position"
            sock.sendall(request.encode("UTF-8")) #Converting string to Byte, and sending it to C#

        receivedData = sock.recv(65536) #receiving data in Byte from C#
        if receivedData:        #If we receive a response, stop sending requests
            responseBool = True
            print(receivedData)



def GUI():
    global running
    global responseBool

    pygame.init()
    boardRenderer()

    while running:
        for event in pygame.event.get():  
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_q:
                    running = False

        if responseBool:        #if a new response is received, we should update the screeen
            boardRenderer()
            responseBool = False



def boardRenderer():

    (width,height)=(800,800)
    screen = pygame.display.set_mode((width,height))

    #Imports All pieces as png logos
    BBishop = pygame.image.load("./Pieces/BBishop.png")
    BKing = pygame.image.load("./Pieces/BKing.png")
    BKnight = pygame.image.load("./Pieces/BKnight.png")
    BPawn = pygame.image.load("./Pieces/BPawn.png")
    BQueen = pygame.image.load("./Pieces/BQueen.png")
    BRook = pygame.image.load("./Pieces/BRook.png")

    WBishop = pygame.image.load("./Pieces/WBishop.png")
    WKing = pygame.image.load("./Pieces/WKing.png")
    WKnight = pygame.image.load("./Pieces/WKnight.png")
    WPawn = pygame.image.load("./Pieces/WPawn.png")
    WQueen = pygame.image.load("./Pieces/WQueen.png")
    WRook = pygame.image.load("./Pieces/WRook.png")

    RendererID = [WPawn, BPawn, WRook, BRook, WKnight, BKnight, WBishop, BBishop, WQueen, BQueen, WKing, BKing]         #list to simplify showing of the pieces
        
    indX = 0
    while indX<8:
        indY=0
        while indY<8:
            if (indX+indY)%2 != 0:
                cValue = 70
            else:
                cValue = 230

            pygame.draw.rect(screen, (cValue,cValue,cValue), pygame.Rect(indX*width/8,indY*height/8,width/8,height/8))  #draw rectangle of correct color
            pygame.display.flip()  
            indY += 1
        indX += 1




requestThread = threading.Thread(target=requestHandler)
GUIThread = threading.Thread(target=GUI)

requestThread.start()
GUIThread.start()