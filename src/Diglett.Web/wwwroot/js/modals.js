(function () {
    function openModal(e) {
        e.preventDefault();

        const url = e.currentTarget.dataset.url;

        fetch(url, { cache: 'no-cache' })
            .then(response => {
                if (!response.ok) throw new Error(`Failed to load ${url}, status: ${response.status}`);
                return response.text();
            })
            .then(html => {
                const modalContent = document.getElementById('mainModalContent');
                modalContent.innerHTML = html;

                const form = modalContent.querySelector('form');
                if (form && typeof $.validator !== 'undefined') {
                    $.validator.unobtrusive.parse(form);
                }

                bootstrap.Modal.getOrCreateInstance(document.getElementById('mainModal')).show();
            })
            .catch(err => console.error('Modal load error:', err));
    }

    document.addEventListener('DOMContentLoaded', () => {
        document.querySelectorAll('.open-modal').forEach(btn => {
            btn.addEventListener('click', openModal);
        });
    });
})();
