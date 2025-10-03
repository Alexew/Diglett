class Picker {
    constructor(options) {
        this.defaults = {
            wrapper: document.querySelector('.page'),
            url: '/Card/Picker'
        };

        this.options = Object.assign({}, this.defaults, options);

        this.init();
    }

    init() {
        fetch(this.options.url)
            .then(response => {
                if (!response.ok) throw new Error(`HTTP error, status: ${response.status}`);
                return response.text();
            })
            .then(html => {
                this.options.wrapper.insertAdjacentHTML('beforeend', html);

                this.picker = this.options.wrapper.querySelector('#card-picker');
                this.list = this.picker.querySelector('.picker-list');
                this.page = this.picker.querySelector('input[name=PageIndex]');
                this.searchInput = this.picker.querySelector('input[name=SearchTerm]');

                delegate(this.picker, '.picker-item', 'click', (_, card) => this.pickCard(card));
                delegate(this.picker, 'button[name=SearchCards]', 'click', () => this.loadCards());

                this.searchInput.addEventListener('keydown', e => {
                    if (e.key === 'Enter') {
                        e.preventDefault();
                        this.loadCards();
                    }
                });
            })
            .catch(console.error);
    }

    loadCards(append = false) {
        if (append) {
            this.page.value = parseInt(this.page.value) + 1;
        } else {
            this.page.value = 0;
        }

        const picker = this.picker;
        const form = picker.querySelector('form');
        const url = form.action;

        fetch(url, {
            method: 'POST',
            body: new FormData(form),
        })
            .then(response => {
                if (!response.ok) throw new Error(`HTTP error, status: ${response.status}`);
                return response.text();
            })
            .then(html => {
                if (append) {
                    this.list.insertAdjacentHTML('beforeend', html);
                } else {
                    this.list.innerHTML = html;
                }

                const sentinel = this.list.querySelector('.load-more');
                if (sentinel) {
                    const observer = new IntersectionObserver(entries => {
                        if (entries.some(entry => entry.isIntersecting)) {
                            sentinel.parentNode.remove();
                            this.loadCards(true);
                        }
                    }, {
                        root: this.list,
                        threshold: 0.0
                    });

                    observer.observe(sentinel);
                }
            })
            .catch(console.error);
    }

    pickCard(card) {
        if (typeof this.options.onSelected === 'function') {
            this.options.onSelected(card.dataset.id);
        }
    }
}

