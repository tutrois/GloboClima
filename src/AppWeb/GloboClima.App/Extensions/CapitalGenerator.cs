namespace GloboClima.App.Extensions
{
    public class CapitalGenerator
    {
        private static readonly List<string> Capitais = new List<string>
        {
            "Rio Branco", // Acre
            "Maceió", // Alagoas
            "Macapá", // Amapá
            "Manaus", // Amazonas
            "Salvador", // Bahia
            "Fortaleza", // Ceará
            "Brasília", // Distrito Federal
            "Vitória", // Espírito Santo
            "Goiânia", // Goiás
            "São Luís", // Maranhão
            "Cuiabá", // Mato Grosso
            "Campo Grande", // Mato Grosso do Sul
            "Belo Horizonte", // Minas Gerais
            "Belém", // Pará
            "João Pessoa", // Paraíba
            "Curitiba", // Paraná
            "Recife", // Pernambuco
            "Teresina", // Piauí
            "Rio de Janeiro", // Rio de Janeiro
            "Natal", // Rio Grande do Norte
            "Porto Alegre", // Rio Grande do Sul
            "Porto Velho", // Rondônia
            "Boa Vista", // Roraima
            "Florianópolis", // Santa Catarina
            "São Paulo", // São Paulo
            "Aracaju", // Sergipe
            "Palmas" // Tocantins
        };

        public static List<string> GerarCapitaisAleatorias(int quantidade = 5)
        {
            var random = new Random();
            var capitaisAleatorias = new HashSet<string>();

            while (capitaisAleatorias.Count < quantidade)
            {
                int index = random.Next(Capitais.Count);
                capitaisAleatorias.Add(Capitais[index]);
            }

            return new List<string>(capitaisAleatorias);
        }
    }
}
