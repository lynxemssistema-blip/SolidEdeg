const { Client } = require('pg'); 
async function run() { 
    const client = new Client({ connectionString: 'postgres://postgres:1401@192.168.1.77:5432/protheus' }); 
    await client.connect(); 
    const res = await client.query("SELECT c6_num, c6_item, c6_pedcli, c6_produto FROM public.sc6010 WHERE c6_num = '002730'"); 
    console.log("SC6010:", res.rows); 
    
    const resZ1 = await client.query("SELECT * FROM public.sz1010 WHERE z1_produto = 'MT6-7283' OR z1_produto = 'MT6-7283   '");
    console.log("SZ1010:", resZ1.rows);
    process.exit(0); 
} 
run();
