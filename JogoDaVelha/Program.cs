using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

class JogoDaVelha
{
    class Jogo
    {
        public Tabuleiro tabuleiro { get; set; }
        public JogadorBase Jogador1 { get; set; }
        public JogadorBase Jogador2 { get; set; }
        public Jogo()
        {
            tabuleiro = new Tabuleiro();
        }
        public JogadorBase PegaProximoJogador(JogadorBase jogadorTurno)
        {
            if (jogadorTurno.PlayerSimbol == Jogador1.PlayerSimbol)
                return Jogador2;
            else
                return Jogador1;
        }
        public void Joga()
        {
            var player = this.Jogador1;
            while (true)
            {
                tabuleiro.PrintTabuleiro();
                player.Joga(this.tabuleiro);
                if (tabuleiro.VerificarGanhador())
                {
                    tabuleiro.PrintTabuleiro();
                    Console.WriteLine(string.Format("Jogador {0} venceu!!!", player.PlayerSimbol));
                    break;

                }else if(tabuleiro.GetJogadasPossiveis().Count == 0)
                {
                    tabuleiro.PrintTabuleiro();
                    Console.WriteLine("Deu Velha!!");
                    break;
                }
               player = PegaProximoJogador(player);
            }
        }
        public int JogaSimulacao(int jogador)
        {
            var NPC = new RandomPlayer();
            while (true)
            {
                NPC.JogaRandom(this.tabuleiro);
                if (tabuleiro.VerificarGanhador())
                {
                    break;
                }
                if (tabuleiro.GetJogadasPossiveis().Count == 0)
                {
                    return -1;
                }
                if (NPC.PlayerSimbol == Tabuleiro.Simbolos.X)
                    NPC.PlayerSimbol = Tabuleiro.Simbolos.O;
                else
                    NPC.PlayerSimbol = Tabuleiro.Simbolos.X;
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
        static int[,] PosicaoDeVitoria = {  { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 },
                                        { 0, 4, 8 }, { 6, 4, 2 }, { 6, 3, 0 },
                                        { 7, 4, 1 }, { 8, 5, 2 }};
        public string[] Table { get; set; }

        public Tabuleiro()
        {
            Table = new string[] { Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio, Tabuleiro.Vazio};
        }
        public bool VerificarGanhador()
        {
            for (int i = 0; i < PosicaoDeVitoria.GetLength(0); i++)
            {
                if (Table[PosicaoDeVitoria[i, 0]] != Tabuleiro.Vazio && Table[PosicaoDeVitoria[i, 1]] != Tabuleiro.Vazio && Table[PosicaoDeVitoria[i, 2]] != Tabuleiro.Vazio)
                {
                    if (Table[PosicaoDeVitoria[i, 0]] == Table[PosicaoDeVitoria[i, 1]] && Table[PosicaoDeVitoria[i, 1]] == Table[PosicaoDeVitoria[i, 2]])
                        return true;
                }

            }
            return false;
        }
        public int VerificarGanhadorMinMax()
        {
            for (int i = 0; i < PosicaoDeVitoria.GetLength(0); i++)
            {
                if (Table[PosicaoDeVitoria[i, 0]] != Tabuleiro.Vazio && Table[PosicaoDeVitoria[i, 1]] != Tabuleiro.Vazio && Table[PosicaoDeVitoria[i, 2]] != Tabuleiro.Vazio)
                {
                    if (Table[PosicaoDeVitoria[i, 0]] == Table[PosicaoDeVitoria[i, 1]] && Table[PosicaoDeVitoria[i, 1]] == Table[PosicaoDeVitoria[i, 2]])
                    {
                        if (Table[PosicaoDeVitoria[i, 0]] == Tabuleiro.Simbolos.X.ToString())
                            return 10;
                        else
                            return -10;

                    }
                }
            }
            return 0;
        }
        public void CopiarTabuleiro(Tabuleiro tabuleiroOld)
        {
            for (int b = 0; b < tabuleiroOld.Table.Length; b++)
            {
                this.Table[b] = tabuleiroOld.Table[b];
            }
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
        public void Joga(Tabuleiro.Simbolos jogador, int posicaoJogada)
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
            Table[posicaoJogada] = jogador.ToString();
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
    class JogadorHumano : JogadorBase
    {
        public override void Joga(Tabuleiro tabuleiro)
        {
            int posicaoJogada;
            while (true)
            {
                Console.WriteLine(string.Format("Jogador ({0}) Digite a posicao para jogar: ", this.PlayerSimbol));
                posicaoJogada = Convert.ToInt16(Console.ReadLine()) - 1;
                if (tabuleiro.GetJogadasPossiveis().Contains(posicaoJogada))
                {
                    break;
                }
                Console.WriteLine("Posicao invalida, selecione uma dessas posicoes: " + tabuleiro.GetJogadasPossiveisString());
            }
            tabuleiro.Joga(this.PlayerSimbol, posicaoJogada);
        }
    }
    class MiniMaxPlayer : JogadorBase
    {
        public override void Joga(Tabuleiro tabuleiro)
        {
            var simulator = new Simulation();
            simulator.TabuleiroSimulation = tabuleiro;
            var pos = simulator.GetBestMove(this.PlayerSimbol, tabuleiro);
            tabuleiro.Joga(this.PlayerSimbol, pos);
        }
    }

    class RandomPlayer : JogadorBase
    {
        public void JogaRandom(Tabuleiro tabuleiro)
        {
            var jogadasAbertas = tabuleiro.GetJogadasPossiveis();
            Random rnd = new Random();
            int r = rnd.Next(jogadasAbertas.Count);
            tabuleiro.Joga(this.PlayerSimbol, jogadasAbertas[r]);
        }
    }
    class MonteCarloIA : JogadorBase
    {
        public override void Joga(Tabuleiro tabuleiro1)
        {
            var simulator = new Simulation();
            simulator.TabuleiroSimulation = tabuleiro1;
            var simulacao = simulator.SimularMonteCarlo();
            tabuleiro1.Joga(this.PlayerSimbol, simulacao);
        }
    }
    class JogadorBase
    {
        public Tabuleiro.Simbolos PlayerSimbol { get; set; }
        public virtual void Joga(Tabuleiro tabuleiro)
        {

        }
    }
    class Simulation
    {
        public static int NumeroDeSimulacoes = 1000;
        public Tabuleiro TabuleiroSimulation;

        public int SimularMonteCarlo()
        {
            IDictionary<int, int> scores = new Dictionary<int, int>();
            var JogadorSimbolo = Enum.GetName(typeof(Tabuleiro.Simbolos), 0);
            foreach (int pos in TabuleiroSimulation.GetJogadasPossiveis())
            {
                scores.Add(pos, 0);
            }
            Jogo jogoSimulado = new Jogo();
            jogoSimulado.tabuleiro.CopiarTabuleiro(TabuleiroSimulation);
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
        public int MiniMax(Tabuleiro tabuleiro, int depth, bool isMax)
        {
            var bestScore = isMax ? -100000 : 100000;
            var score = 0;
            foreach(var move in tabuleiro.GetJogadasPossiveis())
            {
                var currentSymbol = isMax ? Tabuleiro.Simbolos.O : Tabuleiro.Simbolos.X;
                tabuleiro.Joga(currentSymbol, move);
                score = ComputarScore(tabuleiro);
                if(score != 0)
                {
                    tabuleiro.Table[move] = Tabuleiro.Vazio;
                    return score;
                }
                score = MiniMax(tabuleiro, depth + 1, !isMax);
                tabuleiro.Table[move] = Tabuleiro.Vazio;
                if (isMax)
                    bestScore = Math.Max(bestScore, score);
                else
                    bestScore = Math.Min(bestScore, score);

            }
            return score;
        }
        public int GetBestMove(Tabuleiro.Simbolos playerSimbol,Tabuleiro tabuleiro)
        {
            var newBoard = new Tabuleiro();
            newBoard.CopiarTabuleiro(tabuleiro);
            IDictionary<int, int> moves = new Dictionary<int, int>();
            foreach (var move in tabuleiro.GetJogadasPossiveis())
            {
                newBoard.Joga(playerSimbol, move);
                var score = ComputarScore(newBoard);
                if (score != 0)
                    return move;
                else
                    moves.Add(move, MiniMax(newBoard,1,false));
                newBoard.Table[move] = Tabuleiro.Vazio;

            }
            var keyOfMinValue = moves.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;
            return keyOfMinValue;
        }
        public int ComputarScore(Tabuleiro tabuleiro)
        {
            var vencedor = tabuleiro.VerificarGanhadorMinMax();
            if (vencedor != 0)
                return vencedor;
            if (tabuleiro.GetJogadasPossiveis().Count == 0)
                return 0;
            return 0;

        }
    }

    static void Main(string[] args)
    {
        var jogo = new Jogo();
        jogo.Jogador1 = new JogadorHumano();
        jogo.Jogador2 = new MiniMaxPlayer();
        jogo.Jogador1.PlayerSimbol = Tabuleiro.Simbolos.X;
        jogo.Jogador2.PlayerSimbol = Tabuleiro.Simbolos.O;
        jogo.Joga();
    }


}

