function mtgJoin() {
    let callUrl = 'https://huddleafrica.daily.co/hello';//test url call (to be updated!!!)
    if (!window.frame) {
        window.inp = document.getElementById('mtg-link');
        window.frame = window.DailyIframe.createFrame(
            document.getElementById('mtg-frame')
        );
    }
    window.frame.join({ url: callUrl });
};

function mtgLeave() {
    window.frame.leave();
};