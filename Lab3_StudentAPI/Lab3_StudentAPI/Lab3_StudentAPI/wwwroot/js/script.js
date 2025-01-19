let apiUrl = "http://localhost:5232/api/students";
let currentOffset = 0;
let currentLimit = 10;
function getFormat() {
    return document.getElementById('format').value === 'xml' ? '.xml' : '.json';
}

function clearTable() {
    document.querySelector('#student-table tbody').innerHTML = '';
}

function populateTable(students) {
    const tbody = document.querySelector('#student-table tbody');
    clearTable();
    students.forEach(student => {
        const tr = document.createElement('tr');
        tr.innerHTML = `<td>${student.id}</td><td>${student.name}</td><td>${student.phone}</td>`;
        tbody.appendChild(tr);
    });
}

function displayResult(data, format) {
    const linksContainer = document.getElementById('student-links');
    linksContainer.innerHTML = '';
    const pagintaionContainer = document.getElementById('pagination-buttons');
    pagintaionContainer.innerHTML = '';
    const errorContainer = document.getElementById('errorContainer');
    errorContainer.innerHTML = '';

    if (format === '.xml') {
        const parser = new DOMParser();
        const xmlDoc = parser.parseFromString(data, "application/xml");

        if (xmlDoc.getElementsByTagName("Students").length > 0) {
            const students = parseMultipleStudentsXML(xmlDoc);
            populateTable(students.students);
            generatePaginationButtons(students.links);

            // Обновление apiUrl
            const selfLink = students.links.find(link => link.rel === 'GET');
            if (selfLink) apiUrl = selfLink.href;

        } else if (xmlDoc.getElementsByTagName("StudentDto").length === 1) {
            const student = parseSingleStudentXML(xmlDoc);
            populateTable([student]);
            generateStudentLinks(student);
            populateUpdateForm(student);
        }
    } else {
        const jsonData = JSON.parse(data);
        if (jsonData.students) {
            populateTable(jsonData.students);
            generatePaginationButtons(jsonData.links);

            // Обновление apiUrl
            const selfLink = jsonData.links.find(link => link.rel === 'GET');
            if (selfLink) apiUrl = selfLink.href;

        } else if (jsonData.id) {
            populateTable([jsonData]);
            generateStudentLinks(jsonData);
            populateUpdateForm(jsonData);
        }
    }
}

function generateStudentLinks(student) {
    const linksContainer = document.getElementById('student-links');
    linksContainer.innerHTML = ''

    const studentLinks = student.links;
    studentLinks.forEach(link => {
        const button = document.createElement('button');
        button.textContent = link.rel; 
        button.classList.add('blue-button');

        populateUpdateForm(student);
        button.addEventListener('click', () => {
            if (link.method === 'PUT') {
                const studentData = getUpdateFormData();

                const format = getFormat();
                const body = format === '.xml' ? generateStudentXML(studentData) : JSON.stringify(studentData);

                fetchData(link.href, 'PUT', displayResult, body);
            } else if (link.method === 'DELETE') {
                const confirmed = confirm(`Are you sure you want to delete student ${student.name}?`);
                if (confirmed) {
                    fetchData(link.href, 'DELETE', (data, format) => {
                        alert("Student deleted successfully!");
                        clearTable();
                        linksContainer.innerHTML = '';
                    });
                }
            } else {
                fetchData(link.href, link.method, displayResult);
            }
        });

        linksContainer.appendChild(button); 
    });
}



function populateUpdateForm(student) {
    document.getElementById('putID').value = student.id;
    document.getElementById('putName').value = student.name;
    document.getElementById('putPhone').value = student.phone;
}

function parseSingleStudentXML(xmlDoc) {
    const student = {
        id: xmlDoc.getElementsByTagName("ID")[0].textContent,
        name: xmlDoc.getElementsByTagName("Name")[0].textContent,
        phone: xmlDoc.getElementsByTagName("Phone")[0].textContent,
        links: []
    };

    const linkElements = xmlDoc.getElementsByTagName("Link");

    for (let i = 0; i < linkElements.length; i++) {
        const link = {
            href: linkElements[i].getElementsByTagName("Href")[0].textContent,
            rel: linkElements[i].getElementsByTagName("Rel")[0].textContent,
            method: linkElements[i].getElementsByTagName("Method")[0].textContent
        };
        student.links.push(link);
    }

    return student;
}


function parseMultipleStudentsXML(xmlDoc) {
    const studentsXml = xmlDoc.getElementsByTagName("StudentDto");
    const students = [];

    for (let i = 0; i < studentsXml.length; i++) {
        const student = {
            id: studentsXml[i].getElementsByTagName("ID")[0].textContent,
            name: studentsXml[i].getElementsByTagName("Name")[0].textContent,
            phone: studentsXml[i].getElementsByTagName("Phone")[0].textContent
        };
        students.push(student);
    }

    const linksXml = xmlDoc.getElementsByTagName("Link");
    const links = [];
    for (let i = 0; i < linksXml.length; i++) {
        const link = {
            href: linksXml[i].getElementsByTagName("Href")[0].textContent,
            rel: linksXml[i].getElementsByTagName("Rel")[0].textContent,
            method: linksXml[i].getElementsByTagName("Method")[0].textContent
        };
        links.push(link);
    }

    return { students, links };
}

function displayError(errorResponseText) {
    const errorResponse = JSON.parse(errorResponseText);
    const errorContainer = document.getElementById('errorContainer');

    errorContainer.innerHTML = `<div class="error">${errorResponse.message}</div>`;

    const errorLink = errorResponse.links.find(link => link.rel === 'error_info');

    if (errorLink) {
        fetch(errorLink.href, { method: errorLink.method })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to fetch error information');
                }
                return response.json();
            })
            .then(data => {
                const errorContainer = document.getElementById('errorContainer');
                errorContainer.innerHTML = '';

                const errorInfoContainer = document.createElement('div');
                errorInfoContainer.classList.add('error-info');

                errorInfoContainer.innerHTML = `
        <p>Error Code: ${data.error_code}</p>
        <p>Error Description: ${data.error_description}</p>
    `;

                errorContainer.appendChild(errorInfoContainer);

                data.links.forEach(link => {
                    const aTag = document.createElement('a');
                    aTag.href = link.href;
                    aTag.textContent = link.rel;
                    aTag.classList.add('blue-link');
                    aTag.target = '_blank';

                    errorContainer.appendChild(aTag);
                    errorContainer.appendChild(document.createElement('br'));
                });
            })
            .catch(error => {
                console.error('Error fetching additional error info:', error);
                const errorInfoContainer = document.createElement('div');
                errorInfoContainer.classList.add('error-info');
                errorInfoContainer.innerHTML = `<p>Не удалось получить информацию об ошибке.</p>`;
                errorContainer.appendChild(errorInfoContainer);
            });
    }
}


// Event Handlers
document.getElementById('getOneStudent').addEventListener('click', () => {
    const id = document.getElementById('studentID').value;
    fetchData(`${apiUrl}${getFormat()}/${id}`, 'GET', displayResult);
});

document.getElementById('getStudents').addEventListener('click', () => {
    const query = buildQuery();
    fetchData(`${apiUrl}${getFormat()}${query}`, 'GET', displayResult);
});

document.getElementById('postStudent').addEventListener('click', () => {
    const name = document.getElementById('postName').value;
    const phone = document.getElementById('postPhone').value;
    fetchData(apiUrl + ".json", 'POST', displayResult, JSON.stringify({ Name: name, Phone: phone }));
});


document.getElementById('putStudent').addEventListener('click', () => {
    const studentData = getUpdateFormData();

    const format = getFormat();
    const url = `${apiUrl}${format}/${id}`;
    const body = format === '.xml' ? generateStudentXML(studentData) : JSON.stringify(studentData);

    fetchData(url, 'PUT', displayResult, body);
});

function getUpdateFormData() {
    const id = document.getElementById('putID').value;
    const name = document.getElementById('putName').value;
    const phone = document.getElementById('putPhone').value;

    const result = {
        ID: id,
        Name: name,
        Phone: phone
    };
    return result;
}
function generateStudentXML(student) {
    return `
        <StudentDto>
            <ID>${student.ID}</ID>
            <Name>${student.Name}</Name>
            <Phone>${student.Phone}</Phone>
        </StudentDto>
    `;
}

document.getElementById('deleteStudent').addEventListener('click', () => {
    const id = document.getElementById('deleteID').value;
    const url = `${apiUrl}${getFormat()}/${id}`;

    fetchData(url, 'DELETE', (data, format) => {
        if (format === '.json') {
            alert("Student deleted successfully!");
        } else if (format === '.xml') {
            alert("Student deleted successfully!");
        }
        clearTable();
    });
});


// Fetch function
async function fetchData(url, method, callback, body = null) {
    const format = getFormat();
    const headers = {
        'Content-Type': method === 'POST' ? 'application/json' : (format === '.xml' ? 'application/xml' : 'application/json'),
        'Accept': format === '.xml' ? 'application/xml' : 'application/json'
    };

    const options = { method, headers };
    if (body) options.body = body;

    try {
        const response = await fetch(url, options);

        if (response.ok) {
            const data = await response.text();
            callback(data, format);
        } else if (response.status === 204 && method === 'DELETE') {
            callback('', format);
        } else if (response.status === 404) {
            const errorBody = await response.text();
            displayError(errorBody);
        } else {
            throw new Error(`Request failed with status ${response.status}`);
        }
    } catch (error) {
        console.log(error.message);
    }
}



// Pagination buttons
function generatePaginationButtons(links) {
    const paginationContainer = document.getElementById('pagination-buttons');
    paginationContainer.innerHTML = '';
    links.forEach(link => {
        if (link.rel != 'self' && link.rel != 'update_student' && link.rel != 'delete_student') {
            const button = document.createElement('button');
            button.textContent = link.rel;
            button.addEventListener('click', () => {
                fetchData(link.href, 'GET', displayResult);
            });
            paginationContainer.appendChild(button);
        }
    });
}


function buildQuery() {
    const limit = document.getElementById('limit').value || currentLimit;
    const offset = document.getElementById('offset').value || currentOffset;
    const sort = document.getElementById('sort').value || '';
    const minid = document.getElementById('minid').value || '';
    const maxid = document.getElementById('maxid').value || '';
    const like = document.getElementById('like').value || '';
    const globalike = document.getElementById('globalike').value || '';
    const columns = document.getElementById('columns').value || '';

    return `?limit=${limit}&offset=${offset}&sort=${sort}&minid=${minid}&maxid=${maxid}&like=${like}&globalike=${globalike}&columns=${columns}&format=json`;
}
