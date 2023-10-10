function formatarPreco(valor) {
    valor = parseFloat(valor.replace(/\D/g, '')) / 100;
    valor = valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
    return valor;
}

const inputPreco = document.getElementById('preco');

inputPreco.addEventListener('input', function () {
    this.value = formatarPreco(this.value);
});