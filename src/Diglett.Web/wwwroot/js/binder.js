class Binder {
    constructor(element, options) {
        this.defaults = {
            pageCount: 1
        };

        this.binder = element;
        this.options = Object.assign({}, this.defaults, options);
        this.binderId = element.dataset.binderId;
        this.pageNumber = 1;

        delegate(this.binder, '.pager-prev', 'click', () => this.prevPage());
        delegate(this.binder, '.pager-next', 'click', () => this.nextPage());
        delegate(this.binder, '.pick-binder-card', 'click', (_, slot) => this.selectSlot(slot));
        delegate(this.binder, '.delete-binder-card', 'click', (_, btn) => this.removeCard(btn.dataset.slot));
    }

    get slotNumber() {
        const selectedSlot = this.binder.querySelector('.pick-binder-card.selected');

        if (!selectedSlot) return 0;
        return selectedSlot.dataset.slot;
    }

    prevPage() {
        const page = this.pageNumber - 1;
        if (page < 1) return;

        this.loadPage(page);
    }

    nextPage() {
        const page = this.pageNumber + 1;

        this.loadPage(page);
    }

    loadPage(page = this.pageNumber) {
        fetch(`/Binder/LoadPage?id=${this.binderId}&page=${page}`)
            .then(r => r.json())
            .then(data => {
                this.binder.querySelector('.binder-page').innerHTML = data.html;
                this.binder.querySelector('.binder-page-number').textContent = `Page ${data.page}`;

                this.pageNumber = data.page;

                this.updatePagers();
            });
    }

    selectSlot(slot) {
        const selected = slot.classList.contains('selected');

        this.binder.querySelectorAll('.pick-binder-card')
            .forEach(a => a.classList.remove('selected'));

        if (!selected) {
            slot.classList.add('selected');
        }
    }

    addCard(cardId) {
        if (cardId < 1 || this.slotNumber < 1)
            return;

        fetch(`/Binder/AddCard?binderId=${this.binderId}&cardVariantId=${cardId}&slot=${this.slotNumber}`)
            .then(() => this.loadPage());
    }

    removeCard(slotNumber) {
        if (slotNumber < 1) return;

        fetch(`/Binder/RemoveCard?binderId=${this.binderId}&slot=${slotNumber}`)
            .then(() => this.loadPage());
    }

    updatePagers() {
        var prev = this.binder.querySelector('.pager-prev');
        var next = this.binder.querySelector('.pager-next');

        prev.classList.remove('disabled');
        next.classList.remove('disabled');


        if (this.pageNumber == 1)
            prev.classList.add('disabled');

        if (this.pageNumber == this.options.pageCount)
            next.classList.add('disabled');
    }
}
