using System.Collections.Generic;

class JogoDaVelha
{
    class Jogo
    {
        public Tabuleiro tabuleiro { get; set; }
        static int[,] PosicaoDeVitoria = {  { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 },
                                        { 0, 4, 8 }, { 6, 4, 2 }, { 6, 3, 0 },
                                        { 7, 4, 1 }, { 8, 5, 2 }};
        public Jogo()
        {
            tabuleiro = new Tabuleiro();
        }
        public bool VerificarGanhador()
        {
            for (int i = 0; i < PosicaoDeVitoria.GetLength(0); i++)
            {
                if (tabuleiro.Table[PosicaoDeVitoria[i, 0]] != Tabuleiro.Vazio && tabuleiro.Table[PosicaoDeVitoria[i, 1]] != Tabuleiro.Vazio && tabuleiro.Table[PosicaoDeVitoria[i, 2]] != Tabuleiro.Vazio)
                {
                    if (tabuleiro.Table[PosicaoDeVitoria[i, 0]] == tabuleiro.Table[PosicaoDeVitoria[i, 1]] && tabuleiro.Table[PosicaoDeVitoria[i, 1]] == tabuleiro.Table[PosicaoDeVitoria[i, 2]])
                        return true;
                }

            }
            return false;
        }
        public int Joga(int jogador)
        {
            var humano = new JogadorHumano();
            var NPC = new IAPlayer();
            while (true)
            {
                if (jogador == 1)
                {
                    tabuleiro.PrintTabuleiro();
                    humano.Joga(tabuleiro, jogador);
                }
                else
                    NPC.Joga(tabuleiro, jogador);

                if (tabuleiro.GetJogadasPossiveis().Count != 0)
                {
                    if (VerificarGanhador())
                    {
                        tabuleiro.PrintTabuleiro();
                        Console.WriteLine(string.Format("Jogador {0}({1}) venceu!!!", jogador, Enum.GetName(typeof(Tabuleiro.Simbolos), jogador)));
                        break;

                    }
                }
                else
                {
                    Console.WriteLine("Deu Velha!!");
                    break;
                }
                jogador = (jogador + 1) % 2;

            }
            return jogador;
        }
        public int JogaSimulacao(int jogador)
        {
            var NPC = new RandomPlayer();
            while (true)
            {
                NPC.JogaRandom(this.tabuleiro, jogador);
                if (tabuleiro.GetJogadasPossiveis().Count == 0)
                {
                    return -1;
                }
                if (VerificarGanhador())
                {
                    break;
                }
                jogador = (jogador + 1) % 2;
            }
            return jogador;
        }
    }

    class Tabuleiro
    {
        public enum Simbolos
        {
            O,
            X
        };
        static public string Vazio = " ";
        public string[] Table { get; set; }

        public Tabuleiro()
        {
            Table = new string[] { Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio};
        }
        public void PrintTabuleiro()
        {

            var tabuleiroprint = string.Format(" {0} | {1} | {2} \n", Table[6], Table[7], Table[8]);
            tabuleiroprint += "---+---+---\n";
            tabuleiroprint += string.Format(" {0} | {1} | {2} \n", Table[3], Table[4], Table[5]);
            tabuleiroprint += "---+---+---\n";
            tabuleiroprint += string.Format(" {0} | {1} | {2} \n", Table[0], Table[1], Table[2]);
            Console.WriteLine(tabuleiroprint);
        }
        public void Joga(int jogador, int posicaoJogada)
        {
            try
            {
                if (Table[posicaoJogada] != Vazio)
                {
                    Console.WriteLine("Jogada ja foi feita por outro jogador.");
                    Console.WriteLine("Posicoes disponiveis" + GetJogadasPossiveisString());
                    return;
                }
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Posicao invalida");
                Console.WriteLine("Posicoes disponiveis" + GetJogadasPossiveisString());
                return;
            }
            Table[posicaoJogada] = Enum.GetName(typeof(Simbolos), jogador);
        }
        public List<int> GetJogadasPossiveis()
        {
            var listaJogadasLivres = new List<int>();
            for (int i = 0; i < Table.Length; i++)
            {
                if (Table[i] == Vazio)
                {
                    listaJogadasLivres.Add(i);
                }
            }
            return listaJogadasLivres;
        }
        public string GetJogadasPossiveisString()
        {
            var listaJogadasLivres = new List<int>();
            for (int i = 0; i < Table.Length; i++)
            {
                if (Table[i] == Vazio)
                {
                    listaJogadasLivres.Add(i + 1);
                }
            }
            return string.Join("; ", listaJogadasLivres);
        }
    }
    class JogadorHumano
    {
        public void Joga(Tabuleiro tabuleiro, int jogador)
        {
            int posicaoJogada;
            while (true)
            {
                var JogadorSimbolo = Enum.GetName(typeof(Tabuleiro.Simbolos), jogador);
                Console.WriteLine(string.Format("Jogador ({0}) Digite a posicao para jogar: ", JogadorSimbolo));
                posicaoJogada = Convert.ToInt16(Console.ReadLine()) - 1;
                if (tabuleiro.GetJogadasPossiveis().Contains(posicaoJogada))
                {
                    break;
                }
                Console.WriteLine("Posicao invalida, selecione uma dessas posicoes: " + tabuleiro.GetJogadasPossiveisString());
            }
            tabuleiro.Joga(jogador, posicaoJogada);
        }
    }
    class RandomPlayer
    {
        public void JogaRandom(Tabuleiro tabuleiro, int jogador)
        {
            var jogadasAbertas = tabuleiro.GetJogadasPossiveis();
            Random rnd = new Random();
            int r = rnd.Next(jogadasAbertas.Count);
            tabuleiro.Joga(jogador, jogadasAbertas[r]);
        }
    }
    class IAPlayer
    {
        public void Joga(Tabuleiro tabuleiro1, int jogador)
        {
            var simulator = new Simulation();
            simulator.tabuleiro22 = tabuleiro1;
            var simulacao = simulator.Simular();
            tabuleiro1.Joga(jogador, simulacao);
        }
    }
    class Simulation
    {
        public static int NumeroDeSimulacoes = 1000;
        public Tabuleiro tabuleiro22;

        public int Simular()
        {
            IDictionary<int, int> scores = new Dictionary<int, int>();
            var JogadorSimbolo = Enum.GetName(typeof(Tabuleiro.Simbolos), 0);
            foreach (int pos in tabuleiro22.GetJogadasPossiveis())
            {
                scores.Add(pos, 0);
            }
            Jogo jogoSimulado = new Jogo();
            for (int b = 0; b < tabuleiro22.Table.Length; b++)
            {
                jogoSimulado.tabuleiro.Table[b] = tabuleiro22.Table[b];
            }

            for (int i = 0; i < NumeroDeSimulacoes; i++)
            {
                Jogo jogoTeste = new Jogo();
                for (int b = 0; b < jogoSimulado.tabuleiro.Table.Length; b++)
                {
                    jogoTeste.tabuleiro.Table[b] = jogoSimulado.tabuleiro.Table[b];
                }
                var jogadorGanhador = jogoTeste.JogaSimulacao(0);
                var coef = 1;
                if (jogadorGanhador != 0)
                {
                    coef = -1;
                }
                foreach (int pos in jogoSimulado.tabuleiro.GetJogadasPossiveis())
                {
                    if (jogoTeste.tabuleiro.Table[pos] == JogadorSimbolo)
                        scores[pos] += coef;
                    else if (jogoTeste.tabuleiro.Table[pos] != Tabuleiro.Vazio)
                    {
                        scores[pos] -= coef;
                    }
                }

            }
            var keyOfMaxValue = scores.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            string textScores = "";
            foreach(var score in scores)
            {
                textScores += string.Format("P{0}: {1} ", score.Key + 1, score.Value);
            }
            Console.WriteLine(textScores);
            return keyOfMaxValue;
        }
        static void Main(string[] args)
        {
            var tabuleiro = new Tabuleiro();
            var jogo = new Jogo();
            jogo.tabuleiro = tabuleiro;
            jogo.Joga(1);
        }
    }

}

