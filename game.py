import pygame
import sys
import random

# --- Farben ---
WHITE       = (255, 255, 255)
WATER       = (70, 130, 180)
WATER_HOVER = (135, 206, 235)
WATER_DARK  = (25,  25, 112)
HIT         = (220,  50,  50)
MISS        = (90,   90,  90)
GRID_LINE   = (30,   80, 120)
BG          = (15,   35,  70)
GREEN_BTN   = (40,  120,  40)
GREEN_HOV   = (60,  160,  60)

CELL_SIZE  = 65
GRID_SIZE  = 6
PADDING    = 60
INFO_H     = 160

WIN_W = GRID_SIZE * CELL_SIZE + 2 * PADDING
WIN_H = GRID_SIZE * CELL_SIZE + 2 * PADDING + INFO_H


def generate_ships(count):
    ships = set()
    while len(ships) < count:
        ships.add((random.randint(1, GRID_SIZE), random.randint(1, GRID_SIZE)))
    return ships


class Game:
    def __init__(self):
        pygame.init()
        self.screen = pygame.display.set_mode((WIN_W, WIN_H))
        pygame.display.set_caption("Schiffe Versenken")
        self.clock = pygame.time.Clock()

        self.font_lg = pygame.font.SysFont("Arial", 26, bold=True)
        self.font_md = pygame.font.SysFont("Arial", 19)
        self.font_sm = pygame.font.SysFont("Arial", 15)

        self.new_game()

    def new_game(self):
        self.ships    = generate_ships(3)
        self.grid     = {}   # (col, row) -> 'hit' | 'miss'
        self.sunk     = 0
        self.message  = "Klicke auf ein Feld um zu schiessen!"
        self.won      = False

    # --- Hilfsmethoden ---
    def cell_rect(self, col, row):
        x = PADDING + (col - 1) * CELL_SIZE
        y = PADDING + (row - 1) * CELL_SIZE
        return pygame.Rect(x, y, CELL_SIZE, CELL_SIZE)

    def cell_from_mouse(self, mx, my):
        col = (mx - PADDING) // CELL_SIZE + 1
        row = (my - PADDING) // CELL_SIZE + 1
        if 1 <= col <= GRID_SIZE and 1 <= row <= GRID_SIZE:
            return col, row
        return None

    # --- Spiellogik ---
    def shoot(self, col, row):
        if (col, row) in self.grid:
            self.message = "Dieses Feld wurde bereits beschossen!"
            return
        if (col, row) in self.ships:
            self.grid[(col, row)] = 'hit'
            self.sunk += 1
            if self.sunk == 3:
                self.message = "Alle Schiffe versenkt!  Du hast gewonnen!"
                self.won = True
            else:
                self.message = f"GETROFFEN!  ({self.sunk}/3 Schiffe versenkt)"
        else:
            self.grid[(col, row)] = 'miss'
            self.message = "Knapp daneben – nichts getroffen!"

    # --- Zeichnen ---
    def draw(self):
        self.screen.fill(BG)
        mx, my = pygame.mouse.get_pos()
        hover  = self.cell_from_mouse(mx, my)

        # Gitter
        for row in range(1, GRID_SIZE + 1):
            for col in range(1, GRID_SIZE + 1):
                rect  = self.cell_rect(col, row)
                state = self.grid.get((col, row))

                if state == 'hit':
                    color = HIT
                elif state == 'miss':
                    color = MISS
                elif hover == (col, row) and not self.won:
                    color = WATER_HOVER
                else:
                    color = WATER

                pygame.draw.rect(self.screen, color, rect, border_radius=4)
                pygame.draw.rect(self.screen, GRID_LINE, rect, 2, border_radius=4)

                if state == 'hit':
                    t = self.font_lg.render("X", True, WHITE)
                    self.screen.blit(t, t.get_rect(center=rect.center))
                elif state == 'miss':
                    t = self.font_lg.render("O", True, (200, 200, 200))
                    self.screen.blit(t, t.get_rect(center=rect.center))

        # Achsenbeschriftung
        for i in range(1, GRID_SIZE + 1):
            lbl = self.font_sm.render(str(i), True, (160, 210, 255))
            col_r = pygame.Rect(PADDING + (i - 1) * CELL_SIZE, PADDING - 22, CELL_SIZE, 18)
            row_r = pygame.Rect(PADDING - 22, PADDING + (i - 1) * CELL_SIZE, 18, CELL_SIZE)
            self.screen.blit(lbl, lbl.get_rect(center=col_r.center))
            self.screen.blit(lbl, lbl.get_rect(center=row_r.center))

        # Info-Bereich
        iy = PADDING + GRID_SIZE * CELL_SIZE + 18

        ships_lbl = self.font_md.render(f"Schiffe versenkt:  {self.sunk} / 3", True, WHITE)
        self.screen.blit(ships_lbl, (PADDING, iy))

        # Schiff-Icons
        for i in range(3):
            c = HIT if i < self.sunk else (80, 160, 230)
            pygame.draw.rect(self.screen, c, pygame.Rect(PADDING + 215 + i * 38, iy + 4, 30, 16), border_radius=3)

        # Statusmeldung
        if self.won:
            msg_color = (80, 255, 120)
        elif "GETROFFEN" in self.message:
            msg_color = (255, 200, 60)
        else:
            msg_color = (220, 220, 220)

        msg = self.font_md.render(self.message, True, msg_color)
        self.screen.blit(msg, (PADDING, iy + 36))

        # Neues-Spiel-Schaltfläche
        btn = pygame.Rect(PADDING, iy + 80, 165, 42)
        pygame.draw.rect(self.screen, GREEN_HOV if btn.collidepoint(mx, my) else GREEN_BTN, btn, border_radius=8)
        bt = self.font_md.render("Neues Spiel", True, WHITE)
        self.screen.blit(bt, bt.get_rect(center=btn.center))

        pygame.display.flip()
        return btn

    def run(self):
        while True:
            btn = self.draw()

            for event in pygame.event.get():
                if event.type == pygame.QUIT:
                    pygame.quit()
                    sys.exit()

                if event.type == pygame.MOUSEBUTTONDOWN and event.button == 1:
                    mx, my = event.pos
                    if btn.collidepoint(mx, my):
                        self.new_game()
                    elif not self.won:
                        cell = self.cell_from_mouse(mx, my)
                        if cell:
                            self.shoot(*cell)

            self.clock.tick(60)


if __name__ == "__main__":
    Game().run()
