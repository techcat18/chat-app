
function start(){
    const connection = new signalR.HubConnectionBuilder()
        .withUrl('https://localhost:7233/api/videoHub').build();
    
    const peer = new Peer();
    
    peer.on('open', () => {
        const startSignalR = async () => {
            await connection.start();
            await connection.invoke('JoinVideoCallAsync');
        }

        startSignalR();
    })

    const videoArea = document.getElementById('videoArea');
    const videoItem = document.createElement('video');

    videoItem.autoplay = true;
    videoItem.muted = true;

    navigator.mediaDevices.getUserMedia({
        audio: true,
        video: true
    }).then(stream => {
        addVideoStream(videoItem, stream);

        peer.on('call', call => {
            call.answer(stream)
            const video = document.createElement('video')
            
            call.on('stream', userVideoStream => {
                addVideoStream(video, userVideoStream)
            })
        })

        connection.on('UserConnected', userId => {
            connectNewUser(userId, stream);
        })
    });

    const addVideoStream = (video, stream) => {
        video.srcObject = stream;

        video.addEventListener('loadedmetadata', () => {
            video.play();
        })

        videoArea.appendChild(video);
    }
    
    const connectNewUser = (userId, stream) => {
        const call = peer.call(userId, stream)
        const video = document.createElement('video')
        
        call.on('stream', userVideoStream => {
            addVideoStream(video, userVideoStream)
        })
        
        call.on('close', () => {
            video.remove()
        })
    }
}
