create database ITP_Item;
use ITP_Item;
-- Crear la tabla Usuarios
CREATE TABLE Users (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    contraseña VARCHAR(100) NOT NULL,
    rol VARCHAR(20) NOT NULL
);
select * from users;
-- Crear la tabla Items Compartidos
CREATE TABLE Items (
    item_id INT AUTO_INCREMENT PRIMARY KEY,        -- Identificador único del item
    nombre VARCHAR(255) NOT NULL,                   -- Nombre del item
    descripcion TEXT,                              -- Descripción del item
    categoria VARCHAR(50),                          -- Categoría (Ejemplo: Libros, Electrónicos, Asesorías)
    ubicacion VARCHAR(255) NOT NULL,               -- Ubicación dentro del ITP
    horario VARCHAR(100) NOT NULL,                 -- Horario de disponibilidad
    estado VARCHAR(50) DEFAULT 'Disponible',       -- Estado del item (Disponible o Prestado)
    imagen VARCHAR(255),                           -- Ruta o URL de la imagen del item
    usuario_id INT,                                -- Relación con el usuario propietario del item
    FOREIGN KEY (usuario_id) REFERENCES Users(user_id) -- Clave foránea que referencia a la tabla Users
);

-- Insertar usuarios
INSERT INTO Users (nombre, email, contraseña, rol)
VALUES
    ('Juan Pérez', 'juan.perez@itp.edu', '12345', 'Estudiante'),
    ('María López', 'maria.lopez@itp.edu', '67890', 'Estudiante'),
    ('Carlos Gómez', 'carlos.gomez@itp.edu', 'qwerty', 'Estudiante'),
    ('Admin ITP', 'admin@itp.edu', 'adminpass', 'Admin');

-- Insertar items
-- Insertar nuevos items con imagen
INSERT INTO Items (nombre, descripcion, categoria, ubicacion, horario, estado, imagen, usuario_id)
VALUES
    ('Laptop Dell XPS 13', 'Laptop ligera y potente, ideal para programación.', 'Electrónicos', 'Zona común - Edificio A', '9:00 - 17:00', 'Disponible', '/images/laptop_dell_xps13.jpg', 1), -- Juan Pérez
    ('Libro de Matemáticas Discretas', 'Libro con teoría y ejercicios resueltos sobre matemáticas discretas.', 'Libros', 'Salón 103', '8:00 - 10:00', 'Disponible', '/images/libro_matematicas_discretas.jpg', 2), -- María López
    ('Proyector Epson', 'Proyector para presentaciones de 3000 lúmenes.', 'Electrónicos', 'Salón 205', '9:00 - 15:00', 'Disponible', '/images/proyector_epson.jpg', 3), -- Carlos Gómez
    ('Guía de Programación en Python', 'Guía paso a paso para aprender Python desde cero.', 'Libros', 'Zona común - Edificio B', '11:00 - 14:00', 'Disponible', '/images/guia_python.jpg', 4), -- Admin ITP
    ('Arduino Uno', 'Placa de desarrollo Arduino Uno para proyectos electrónicos.', 'Electrónicos', 'Salón 301', '10:00 - 12:00', 'Disponible', '/images/arduino_uno.jpg', 1); -- Juan Pérez

update items set imagen = 'imagenes/compphp.jpeg' where item_id = 5;

select * from items;
-- Crear una vista para el reporte de items disponibles para compartir
CREATE VIEW reporte_items_disponibles AS
SELECT 
    i.item_id,
    i.nombre AS item_nombre,
    i.descripcion AS item_descripcion,
    i.categoria AS item_categoria,
    i.ubicacion AS item_ubicacion,
    i.horario AS item_horario,
    i.estado AS item_estado,
    i.imagen AS item_imagen,
    u.nombre AS propietario_nombre,
    u.email AS propietario_email,
    u.rol AS propietario_rol
FROM 
    Items i
JOIN 
    Users u ON i.usuario_id = u.user_id
WHERE 
    i.estado = 'Disponible';

-- Consultar el reporte de items disponibles para compartir
SELECT * FROM reporte_items_disponibles;


DELIMITER $$

CREATE PROCEDURE VerificarEstudiante(IN p_nombre VARCHAR(255), IN p_contraseña VARCHAR(255))
BEGIN
    DECLARE estudiante_existente INT DEFAULT 0;

    -- Verificar si existe un estudiante con el nombre y contraseña proporcionados
    SELECT COUNT(*) INTO estudiante_existente
    FROM Users
    WHERE nombre = p_nombre AND contraseña = p_contraseña AND rol = 'Estudiante';

    -- Si el estudiante existe, devolver un mensaje con su nombre y email
    IF estudiante_existente > 0 THEN
        SELECT nombre, email, rol FROM Users WHERE nombre = p_nombre AND contraseña = p_contraseña AND rol = 'Estudiante';
    ELSE
        -- Si no existe, devolver un mensaje de error
        SELECT 'NO EXISTE ESTUDIANTE' AS mensaje;
    END IF;
END$$

DELIMITER ;
-- Llamar al procedimiento para verificar al estudiante
CALL VerificarEstudiante('Juan Pérez', '12345');
select * from users;
-- AGREGAR USUARIO
DELIMITER $$

CREATE PROCEDURE AgregarUsuario(
    IN p_nombre VARCHAR(255),
    IN p_email VARCHAR(255),
    IN p_contraseña VARCHAR(255),
    IN p_rol VARCHAR(50)
)
BEGIN
    -- Verificar si ya existe un usuario con el mismo email
    IF EXISTS (SELECT 1 FROM Users WHERE email = p_email) THEN
        SELECT 'YA EXISTE UN USUARIO CON ESE EMAIL' AS mensaje;

    -- Verificar si ya existe un usuario con la misma contraseña
    ELSEIF EXISTS (SELECT 1 FROM Users WHERE contraseña = p_contraseña) THEN
        SELECT 'ESA CONTRASEÑA YA ESTÁ EN USO' AS mensaje;

    -- Si no existe ni el email ni la contraseña, insertar el nuevo usuario
    ELSE
        INSERT INTO Users (nombre, email, contraseña, rol)
        VALUES (p_nombre, p_email, p_contraseña, p_rol);

        SELECT 'USUARIO REGISTRADO CORRECTAMENTE' AS mensaje;
    END IF;
END$$

DELIMITER ;

drop procedure agregarusuario;
CALL AgregarUsuario('Rodrigo Ortega', 'rodrigo.ort@itp.edu', '123456', 'Estudiante');

-- MODIFICAR USUARIO
DELIMITER $$

CREATE PROCEDURE ModificarUsuario(
    IN p_usuario_id INT,
    IN p_nombre VARCHAR(255),
    IN p_email VARCHAR(255),
    IN p_contraseña VARCHAR(255),
    IN p_rol VARCHAR(50)
)
BEGIN
    -- Verificar si el usuario existe
    IF EXISTS (SELECT 1 FROM Users WHERE user_id = p_usuario_id) THEN
        -- Actualizar los datos del usuario
        UPDATE Users
        SET nombre = p_nombre,
            email = p_email,
            contraseña = p_contraseña,
            rol = p_rol
        WHERE user_id = p_usuario_id;

        SELECT 'DATOS DEL USUARIO ACTUALIZADOS' AS mensaje;
    ELSE
        SELECT 'USUARIO NO ENCONTRADO' AS mensaje;
    END IF;
END$$

DELIMITER ;

CALL ModificarUsuario(3, 'Carlos Gómez Torres', 'carlos.gomez@itp.edu', 'nuevaclave999', 'Docente');
drop procedure modificarusuario;
-- ELIMINAR USUARIO
DELIMITER $$

CREATE PROCEDURE EliminarUsuario(
    IN p_user_id INT
)
BEGIN
    -- Verificar si el usuario existe
    IF EXISTS (SELECT 1 FROM Users WHERE user_id = p_user_id) THEN
        -- Eliminar los items relacionados al usuario
        DELETE FROM Items WHERE usuario_id = p_user_id;

        -- Eliminar el usuario
        DELETE FROM Users WHERE user_id = p_user_id;

        SELECT 'USUARIO Y SUS ITEMS ELIMINADOS CORRECTAMENTE' AS mensaje;
    ELSE
        SELECT 'USUARIO NO ENCONTRADO' AS mensaje;
    END IF;
END$$

DELIMITER ;

CALL EliminarUsuario(2);
drop procedure eliminarusuario;
DELIMITER $$

CREATE PROCEDURE ActualizarEstadoItem(
    IN p_item_id INT,
    IN p_nuevo_estado VARCHAR(50)
)
BEGIN
    DECLARE v_estado_actual VARCHAR(50);

    -- Verificar si el item existe
    IF EXISTS (SELECT 1 FROM Items WHERE item_id = p_item_id) THEN

        -- Obtener el estado actual del ítem
        SELECT estado INTO v_estado_actual
        FROM Items
        WHERE item_id = p_item_id;

        -- Si se quiere prestar
        IF p_nuevo_estado = 'prestado' THEN
            IF v_estado_actual = 'Disponible' THEN
                UPDATE Items
                SET estado = 'No disponible'
                WHERE item_id = p_item_id;

                SELECT 'El ítem ha sido prestado y ahora está marcado como No disponible' AS mensaje;
            ELSE
                SELECT 'El ítem no está disponible para préstamo' AS mensaje;
            END IF;

        -- Si se quiere devolver
        ELSEIF p_nuevo_estado = 'devuelto' THEN
            IF v_estado_actual = 'No disponible' THEN
                UPDATE Items
                SET estado = 'Disponible'
                WHERE item_id = p_item_id;

                SELECT 'El ítem ha sido devuelto y ahora está marcado como Disponible' AS mensaje;
            ELSE
                SELECT 'El ítem ya está disponible. No se requiere devolución' AS mensaje;
            END IF;

        ELSE
            SELECT 'Estado no reconocido. Usa "prestado" o "devuelto"' AS mensaje;
        END IF;

    ELSE
        SELECT 'No se encontró el ítem con ese ID' AS mensaje;
    END IF;
END$$

DELIMITER ;


drop procedure ActualizarEstadoInteraccion;
CALL ActualizarEstadoItem(5, 'devuelto');

select* from items;

select * from interactions;
select * from items;
SELECT * FROM reporte_items_disponibles;

-- AGREGAR UN NUEVO ITEM
DELIMITER $$

CREATE PROCEDURE AgregarItem(
    IN p_nombre VARCHAR(255),
    IN p_descripcion TEXT,
    IN p_categoria VARCHAR(100),
    IN p_ubicacion VARCHAR(255),
    IN p_horario VARCHAR(100),
    IN p_imagen VARCHAR(255),
    IN p_usuario_id INT
)
BEGIN
    -- Verificar si el usuario existe
    IF EXISTS (SELECT 1 FROM Users WHERE user_id = p_usuario_id) THEN
        -- Insertar el nuevo item
        INSERT INTO Items (
            nombre,
            descripcion,
            categoria,
            ubicacion,
            horario,
            estado,
            imagen,
            usuario_id
        )
        VALUES (
            p_nombre,
            p_descripcion,
            p_categoria,
            p_ubicacion,
            p_horario,
            'Disponible', -- Estado por defecto
            p_imagen,
            p_usuario_id
        );

        SELECT 'ITEM AGREGADO CORRECTAMENTE' AS mensaje;
    ELSE
        SELECT 'NO SE ENCONTRÓ EL USUARIO. NO SE PUDO AGREGAR EL ITEM' AS mensaje;
    END IF;
END$$

DELIMITER ;


CALL AgregarItem(
    'Calculadora científica Casio FX-991ES',
    'Calculadora con funciones avanzadas, útil para ingeniería.',
    'Accesorios',
    'Edificio C - Zona común',
    '10:00 - 13:00',
    '/images/calculadora_casio.jpg',
    2 -- ID del usuario que ya debe estar registrado
);

-- BUSCAR ALGUN USUARIO
DELIMITER $$

CREATE PROCEDURE BuscarUsuario (
    IN p_parametro VARCHAR(100)
)
BEGIN
    -- Si el parámetro es un número (ID)
    IF p_parametro REGEXP '^[0-9]+$' THEN
        SELECT * FROM Users WHERE user_id = CAST(p_parametro AS UNSIGNED);

    -- Si es un texto, buscar por nombre (parcial)
    ELSEIF p_parametro IS NOT NULL THEN
        SELECT * FROM Users WHERE nombre LIKE CONCAT('%', p_parametro, '%');

    -- Si el parámetro está vacío
    ELSE
        SELECT 'Debe ingresar un nombre o un ID de usuario válido' AS mensaje;
    END IF;
END$$

DELIMITER ;

drop procedure BuscarUsuario;
CALL BuscarUsuario('1');
CALL BuscarUsuario(NULL);

-- BUSCAR ITEM POR NOMBRE
DELIMITER $$

CREATE PROCEDURE BuscarItemPorNombre(
    IN p_nombre_item VARCHAR(255)
)
BEGIN
    IF p_nombre_item IS NOT NULL AND p_nombre_item <> '' THEN
        SELECT 
            i.item_id,
            i.nombre AS item_nombre,
            i.descripcion,
            i.categoria,
            i.ubicacion,
            i.horario,
            i.estado,
            i.imagen,
            u.nombre AS propietario
        FROM 
            Items i
        JOIN 
            Users u ON i.usuario_id = u.user_id
        WHERE 
            i.nombre LIKE CONCAT('%', p_nombre_item, '%');
    ELSE
        SELECT 'DEBES INGRESAR UN NOMBRE DE ITEM VÁLIDO' AS mensaje;
    END IF;
END$$

DELIMITER ;

CALL BuscarItemPorNombre('Arduino');
select * from users;


DELIMITER $$

CREATE PROCEDURE ObtenerUsuarios()
BEGIN
    SELECT user_id, nombre, email, rol
    FROM Users;
END$$

DELIMITER ;

select * from items;


DELIMITER $$

CREATE PROCEDURE MostrarItems()
BEGIN
    SELECT 
        i.item_id,
        i.nombre,
        i.descripcion,
        i.categoria,
        i.ubicacion,
        i.horario,
        i.estado,
        i.imagen,
        i.usuario_id,
        u.nombre AS nombre_usuario,
        u.email
    FROM Items i
    INNER JOIN Users u ON i.usuario_id = u.user_id;
END $$

DELIMITER ;


CREATE VIEW Vista_Usuarios AS
SELECT user_id, nombre, email, rol
FROM Users;

select * from Vista_Usuarios;

CREATE VIEW Vista_Items AS
SELECT 
    i.item_id,
    i.nombre,
    i.descripcion,
    i.categoria,
    i.ubicacion,
    i.horario,
    i.estado,
    i.imagen,
    i.usuario_id,
    u.nombre AS nombre_usuario,
    u.email
FROM Items i
INNER JOIN Users u ON i.usuario_id = u.user_id;

SELECT * FROM Vista_Items;

//BUACAR POR ID ITEM
DELIMITER $$

CREATE PROCEDURE BuscarItemPorID(
    IN p_item_id INT
)
BEGIN
    IF p_item_id IS NOT NULL AND p_item_id > 0 THEN
        SELECT 
            i.item_id,
            i.nombre AS item_nombre,
            i.descripcion,
            i.categoria,
            i.ubicacion,
            i.horario,
            i.estado,
            i.imagen,
            u.nombre AS propietario
        FROM 
            Items i
        JOIN 
            Users u ON i.usuario_id = u.user_id
        WHERE 
            i.item_id = p_item_id;
    ELSE
        SELECT 'DEBES INGRESAR UN ID DE ITEM VÁLIDO' AS mensaje;
    END IF;
END$$

DELIMITER ;

CALL BuscarItemPorID(1);

select* from interactions;

select* from items;