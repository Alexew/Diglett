function delegate(parent, selector, type, handler, preventDefault = true) {
    parent.addEventListener(type, function (event) {
        const targetElement = event.target.closest(selector);
        if (targetElement && parent.contains(targetElement)) {
            if (preventDefault) event.preventDefault();
            handler.call(targetElement, event, targetElement);
        }
    });
}
