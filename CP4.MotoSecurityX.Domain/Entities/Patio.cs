namespace CP4.MotoSecurityX.Domain.Entities
{
    public class Patio
    {
        private readonly List<Moto> _motos = new();
        private Patio() { } // EF

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Nome { get; private set; } = "";
        public string Endereco { get; private set; } = "";

        public IReadOnlyCollection<Moto> Motos => _motos.AsReadOnly();

        public Patio(string nome, string endereco)
        {
            if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome inválido", nameof(nome));
            if (string.IsNullOrWhiteSpace(endereco)) throw new ArgumentException("Endereço inválido", nameof(endereco));
            Nome = nome.Trim();
            Endereco = endereco.Trim();
        }

        /// <summary>
        /// Admite a moto neste pátio, mantendo consistência do agregado.
        /// </summary>
        public void AdmitirMoto(Moto moto)
        {
            if (moto is null) throw new ArgumentNullException(nameof(moto));

            // se já está neste pátio, não duplica nem reatribui desnecessariamente
            if (moto.DentroDoPatio && moto.PatioId == Id)
                return;

            moto.EntrarNoPatio(Id);

            if (!_motos.Any(m => m.Id == moto.Id))
                _motos.Add(moto);
        }

        /// <summary>
        /// Remove a moto deste pátio, se presente.
        /// </summary>
        public void RemoverMoto(Moto moto)
        {
            if (moto is null) throw new ArgumentNullException(nameof(moto));

            // só "sai do pátio" se ela estava listada aqui
            if (_motos.RemoveAll(m => m.Id == moto.Id) > 0)
                moto.SairDoPatio();
        }

        public void AtualizarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome inválido.", nameof(nome));

            Nome = nome.Trim();
        }

        public void AtualizarEndereco(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco))
                throw new ArgumentException("Endereço inválido.", nameof(endereco));

            Endereco = endereco.Trim();
        }
    }
}


