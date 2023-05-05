let localStream = undefined;
let myVideoArea = undefined;
let myPeer = undefined;
let myPeerId = undefined;

function createPeer(){
    return new Peer();
}

function getPeer(){
    return myPeer;
}

function acceptCall(instance){
    myPeer = createPeer();
    /*if (!myPeer){
        myPeer = createPeer();
    }*/

    myPeer.on('open', id => {
        instance.invokeMethodAsync("AcceptCallAsync", id);
    })
}

function initialize(chatId, instance){
    window.addEventListener('unload', stopMultimedia);
    
    const peer = new Peer();

    peer.on('open', async id => {
        instance.invokeMethodAsync('JoinVideoCallAsync', id);
    })

    myPeer = peer;
    return peer;
}

function getLocalStream(){
    return localStream;
}

function initializeUserCall(userId, instance){
    window.addEventListener('unload', stopMultimedia)

    const peer = new Peer();
    
    peer.on('open', async id => {
        instance.invokeMethodAsync("CallUserAsync", id, userId);
    })

    myPeer = peer;
    return peer;
}

async function leaveCall(connection, chatId) {
    await connection.invoke('LeaveVideoCallAsync', chatId);
}

async function start(peer){
    const videoArea = document.getElementById('videoArea');
    myVideoArea = videoArea
    
    const myVideo = document.createElement('video');
    
    videoArea.append(myVideo)

    await navigator.mediaDevices.getUserMedia({
        video: true,
        audio: true
    }).then(stream => {
        localStream = stream

        peer.on('call', call => {
            call.answer(stream)
            const video = document.createElement('video')

            call.on('stream', userVideoStream => {
                addVideoStream(video, userVideoStream)
            })
        })
        
        addVideoStream(myVideo, stream)
    })
}

function addVideoStream(video, stream){
    video.srcObject = stream;

    video.addEventListener('loadedmetadata', () => {
        video.play();
    })

    myVideoArea.appendChild(video);
}

function connectToNewUser(userId, peer, stream){
    console.log('connecting to new user', userId)
    const call = peer.call(userId, stream)
    const video = document.createElement('video')

    console.log("peer", peer);
    console.log("stream", stream);

    call.on('stream', userVideoStream => {
        console.log('on stream')
        addVideoStream(video, userVideoStream, myVideoArea)
    })

    call.on('close', () => {
        console.log('on close')
        video.remove()
    });
}

function stop(peer){
    peer.destroy()
    stopMultimedia();
}

function stopMultimedia(){
    localStream.getTracks().forEach(function(track) {
        track.stop();
    });
}

function getJwtToken(){
    let jwtToken = localStorage.getItem('token');
    jwtToken = jwtToken.substring(1);
    jwtToken = jwtToken.substring(0, jwtToken.lastIndexOf('"'));
    
    return jwtToken;
}