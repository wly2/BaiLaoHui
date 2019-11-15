local util = require 'xlua.util'

local yield_return = (require 'cs_coroutine').yield_return

local co = coroutine.create(function()
    print('coroutine start!')
    local s = os.time()
    yield_return(CS.UnityEngine.WaitForSeconds(3))
    print('wait interval:', os.time() - s)
    
    local www = CS.UnityEngine.WWW('http://www.qq.com')
    yield_return(www)
	if not www.error then
        print(www.bytes)
	else
	    print('error:', www.error)
	end
end)

assert(coroutine.resume(co))
