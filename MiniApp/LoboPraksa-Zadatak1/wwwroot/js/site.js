
 function loadRole()
 {
	 const role = getRole();
	 
	 if (role=="admin" || role=="user")
	 {
		document.getElementById('logIn').innerHTML=" ";
		const user = getUser();
		document.getElementById('loggerUser').innerHTML="Dear "+user+", we hope that you'll find a good book. Good luck!";
		document.getElementById('loggerUser').style.background="rgb(237, 229, 178)";
	 }
	
 }



 function signIn(){
	const uriSignIn = 'Login/loginUser';
	async function checkUser() {
	const user = document.getElementById('username').value.trim();
	const pass = document.getElementById('password').value.trim();
	const item = {
    username:user,
    password:pass
    };
	
	
	const response = await  fetch(uriSignIn , {
    method: 'POST',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(item)
    });	
	return response.json();
}

	checkUser()
		.then(data => {
		sessionStorage.setItem("token", data.token);
		sessionStorage.setItem("role", data.role);
		sessionStorage.setItem("username", data.username);
		loadRole();
	}) 
	.catch(error => console.error('Unable to get items.', error));
 }
 
 
 function getToken()
 {
	 const token = sessionStorage.getItem("token");
     return token;
 }
 
 function getRole()
 {
	 const role = sessionStorage.getItem("role");
     return role;
 }
 
 function getUser()
 {
	 const role = sessionStorage.getItem("username");
     return role;
 }
 
 
 function showGenreWithBooks() {
  const uriBook = "Book/getBooksByGenre";
  fetch(uriBook)
    .then(response => response.json())
    .then(data => _displayGenreWITHBooks(data))
    .catch(error => console.error('Unable to get items.', error));
}


 function ReadFreeBook()
 {
	 const role = getRole();
	 if (role !="admin" && role!="user")
	 {
		document.getElementById("bodyApp").innerHTML="Sorry, you need to be logged in to see this page"; 
	 }
	 else{
		 const uriList="GoogleDrive/listFiles";
		 fetch(uriList)
		.then(response => response.json())
		.then(data => _displayFreeBooks(data))
		.catch(error => console.error('Unable to get items.', error));
		var upload = document.getElementById("logIn");
		const secret = document.getElementById("secret").value;
		if (secret != 1)
		{
			let uploadButton = document.createElement("button");
			uploadButton.innerText = 'Upload your pdf';
			uploadButton.style.marginLeft="5px";
			uploadButton.style.paddingBottom="10px";
			uploadButton.style.paddingTop="10px";
			uploadButton.style.fontWeight = "bold";
			uploadButton.style.background="transparent";
			uploadButton.style.fontSize="20px";
			uploadButton.setAttribute('onclick','_uploadBook()');
			upload.appendChild(uploadButton);
		}
		document.getElementById("secret").value=1;
		
	 }
	 
	 
 }
 
function SearchAuthors()
{
	const uriAuthor='Author/getAuthors';
	 fetch(uriAuthor)
    .then(response => response.json())
    .then(data => _displayAuthors(data))
    .catch(error => console.error('Unable to get items.', error));
	
}
 
 function _uploadBook(){
    const tBody = document.getElementById('bodyApp');
	tBody.innerHTML='';
	const input = document.createElement("input");
	let textNode = document.createTextNode("Enter the address and name of your pdf: ");
    tBody.appendChild(textNode);
    input.setAttribute('type', 'text');
	tBody.appendChild(input);
	let done = document.createElement("button");
	done.innerText = 'Done';
	tBody.appendChild(done);
	done.onclick=function(){
		const uriUpload='GoogleDrive/uploadFile?path='+input.value;
		tBody.innerHTML+=uriUpload;
		 fetch(uriUpload)
			.then(
			tBody.innerHTML='File uploaded successfully. You can see and download it in the Read free books section'
			)
			.catch(error => console.error('Unable to get items.', error));
	}
 	
    	
    
 }
 
 function _displayAuthors(data)
{
    const tBody = document.getElementById('bodyApp');
	tBody.innerHTML='';
	const role = getRole();
	const token = getToken();
	
	if (role=='admin')
	{
		document.getElementById("secret").value=1;
	}
    const ind = document.getElementById("secret").value;
		var div = document.createElement("div");
		div.style.width = "100%";
		div.style.height = "1000px";
		div.style.background = "rgb(235, 217, 185)";
    data.forEach(item=>{
		var newDiv = document.createElement("div");
		newDiv.style.width = "auto";
		newDiv.style.height = "auto";
		newDiv.style.background = "rgb(235, 217, 185)";
		newDiv.style.fontWeight = "bold";
		newDiv.style.fontSize = "20px";
		newDiv.style.float="left";
		
		let textNode = document.createTextNode(item.firstName);
		newDiv.appendChild(textNode); 
		
		let blank = document.createTextNode(" ");
		newDiv.appendChild(blank); 
		
		let textNode1 = document.createTextNode(item.lastName);
		newDiv.appendChild(textNode1);
			
		if (ind == 1)
		{
			
			let deleteButton = document.createElement("button");
			deleteButton.innerText = 'Delete Authors';
			deleteButton.style.marginLeft="5px";
			deleteButton.style.paddingBottom="5px";
			deleteButton.style.paddingTop="5px";
			deleteButton.onclick=  function(){
				 const uriDelete = 'Author/deleteAuthor';
				 const idI=item.id
				 const itemJSON = {
					id: idI
				 };
	            
				 fetch(uriDelete , {
						method: 'POST',
						headers: {
						  'Accept': 'application/json',
						  'Content-Type': 'application/json',
						  'Authorization':'Bearer '+token
						  
						},
						body: JSON.stringify(itemJSON)
						})
					  .then()
					  .catch(error => console.error('Unable to delete item.', error));
				}

            newDiv.appendChild(deleteButton);			
		} 
		div.appendChild(newDiv);
		
		var between = document.createElement("div");
		between.style.width = "100%";
		between.style.height = "50px";
		div.append(between);
	});
	
	tBody.appendChild(div);
	
	
}

 
function _displayGenreWITHBooks(data) {
  const tBody = document.getElementById('bodyApp');
  tBody.innerHTML = '';
  
    data.forEach(item => {
    
    var newDiv = document.createElement("div");
    newDiv.style.width = "100%";
    newDiv.style.height = "auto";
    newDiv.style.background = "rgb(235, 217, 185)";
    newDiv.style.fontWeight = "bold";
    newDiv.style.fontSize = "35px";
    newDiv.style.float="left";
    newDiv.style.textAlign = "center";

    let textNode = document.createTextNode(item.genreName);
    newDiv.appendChild(textNode);  
    
    var between = document.createElement("div");
    between.style.width = "100%";
    between.style.height = "10px";
    newDiv.append(between);
    
    var bookDiv = document.createElement("div");
    bookDiv.style.height="200px";
    bookDiv.style.width="90%";
    bookDiv.style.marginLeft="30px";
    bookDiv.style.marginRight="30px";
    
    
    item.books.forEach(book=>{
    var newDiv1 = document.createElement("div");
    var img = document.createElement('img'); 
    img.src ='images/knjiga'+book.id+'.jpg';
    img.style.width="200px";
    img.style.height="200px";
    
    newDiv1.style.fontWeight = "bold";
    newDiv1.style.marginLeft="10px";
    newDiv1.style.marginRight="10px";
    newDiv1.style.textAlign = "center";
    newDiv1.style.float="left";
    newDiv1.style.overflowX="auto";
    newDiv1.append(img);
    bookDiv.appendChild(newDiv1);
    
    });
    
    newDiv.append(bookDiv); 
    tBody.append(newDiv);
   });

  todos = data;
}

 
 function _displayFreeBooks(data)
 {
	const tBody = document.getElementById('bodyApp');
    tBody.innerHTML = '';
	var div = document.createElement("div");
	div.style.width = "100%";
	div.style.height = "1000px";
	div.style.background = "rgb(235, 217, 185)";
	
	data.forEach(item => {
    var newDiv = document.createElement("div");
    newDiv.style.width = "auto";
    newDiv.style.height = "auto";
    newDiv.style.background = "rgb(235, 217, 185)";
    newDiv.style.fontWeight = "bold";
    newDiv.style.fontSize = "20px";
    newDiv.style.float="left";
	
	var img = document.createElement('img'); 
    img.src ='images/pdf.png';
    img.style.width="100px";
    img.style.height="100px";
	newDiv.appendChild(img);
	
	let downloadButton = document.createElement("button");
    downloadButton.innerText = 'Download';
	downloadButton.style.marginLeft="5px";
	downloadButton.style.paddingBottom="10px";
	downloadButton.style.paddingTop="10px";
	downloadButton.style.fontWeight = "bold";
	downloadButton.onclick=function(){
		 const uriDownload = 'GoogleDrive/downloadFile?id='+item.id+'&savePath=C:\\Users\\Nevermind\\Desktop\\download\\'+item.id+'.pdf';		 
		 fetch(uriDownload)
		.then()
		.catch(error => console.error('Unable to get items.', error));
	}
	
	let textNode = document.createTextNode(item.name);
    newDiv.appendChild(textNode); 
	newDiv.appendChild(downloadButton);
     
	div.appendChild(newDiv);
	});
	
	tBody.appendChild(div);
	
	
 }















