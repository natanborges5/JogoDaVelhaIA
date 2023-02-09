//Inicio do jogo da velha

string[,] matriz = new string[3, 3] { 
    { "null", "null", "null" },
    { "null", "null", "null" },
    { "null", "null", "null" } };
bool jogoRolando = true;
int jogador = 1;

while (jogoRolando)
{
    PrintarJogo();
    JogaPlayer(jogador);
    jogador = (jogador + 1) % 2;

}
void PrintarJogo()
{
    int rowLength = matriz.GetLength(0);
    int colLength = matriz.GetLength(1);

    for (int i = 0; i < rowLength; i++)
    {
        for (int j = 0; j < colLength; j++)
        {
            Console.Write(string.Format("{0} ", matriz[i, j] + " | "));
        }
        Console.Write(Environment.NewLine + Environment.NewLine);
    }
};
void JogaPlayer(int jogador)
{
    Console.WriteLine("Digite a coluna selecionada: ");
    var coluna = Convert.ToInt16(Console.ReadLine()) -1;
    Console.WriteLine("Digite a linha selecionada: ");
    var linha = Convert.ToInt16(Console.ReadLine()) -1;
    if (coluna < 0 || coluna > 2)
    {
        Console.WriteLine("Coluna invalida");
        JogaPlayer(jogador);
    }
    if( linha < 0 || linha > 2)
    {
        Console.WriteLine("Linha invalida");
        JogaPlayer(jogador);
    }

    if (matriz[linha,coluna] == "null")
    {
        matriz[linha, coluna] = Enum.GetName(typeof(Simbolos), jogador);
    }
    else
    {
        Console.WriteLine("Posição já foi escolhido por um jogador");
        JogaPlayer(jogador);
    }
        


}
enum Simbolos
{
    O,
    X
};